using DotNetEnv;
using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaladinHub.Controllers;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Repositories.Contracts;
using PaladinHub.Infrastructure;
using PaladinHub.Infrastructure.Routing;
using PaladinHub.Services;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Discussions;
using PaladinHub.Services.PageBuilder;
using PaladinHub.Services.Presets;
using PaladinHub.Services.SectionServices;
using PaladinHub.Services.ServiceExtension;
using PaladinHub.Services.TalentTrees;
using StackExchange.Redis;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllersWithViews(options =>
	{
		options.Filters.Add<LoadGlobalDataFilter>();
	})
	.AddRazorOptions(o =>
	{
		o.ViewLocationExpanders.Add(new SectionViewLocationExpander());
	});

builder.Services.AddSession();
builder.Services.AddCustomServices();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISpellbookService, SpellbookService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartSessionService, CartSessionService>();
builder.Services.AddScoped<IBlockRenderer, BlockRenderer>();
builder.Services.AddScoped<ISpecializationTreeBuilder, HolySpecTreeBuilder>();
builder.Services.AddScoped<ISpecializationTreeBuilder, ProtectionSpecTreeBuilder>();
builder.Services.AddScoped<ISpecializationTreeBuilder, RetributionSpecTreeBuilder>();
builder.Services.AddScoped<IClassTreeBuilder, PaladinClassTreeBuilder>();
builder.Services.AddScoped<IHeroTalentTreesService, HeroTalentTreesService>();
builder.Services.AddScoped<ITalentTreeService, TalentTreeService>();
builder.Services.AddHostedService<PaladinHub.Services.Background.CleanupCartService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IJsonLayoutValidator, JsonLayoutValidator>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IDataPresetService, DataPresetService>();

builder.Services.AddSingleton<IHtmlSanitizer>(_ =>
{
	var s = new HtmlSanitizer();
	s.AllowedTags.Add("p"); s.AllowedTags.Add("b"); s.AllowedTags.Add("i");
	s.AllowedTags.Add("ul"); s.AllowedTags.Add("ol"); s.AllowedTags.Add("li");
	s.AllowedTags.Add("a"); s.AllowedAttributes.Add("href");
	return s;
});

builder.Services.Configure<RouteOptions>(opt =>
{
	opt.ConstraintMap["palsec"] = typeof(AllowedSectionConstraint);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
	string conn = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
	options.UseNpgsql(conn);
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
			 ?? throw new InvalidOperationException("JWT_KEY is missing from .env");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "DefaultIssuer";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "DefaultAudience";

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(o =>
	{
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtIssuer,
			ValidAudience = jwtAudience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
		};
	});

string redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
builder.Services.AddScoped<ICartStore, RedisCartStore>();

builder.Services.AddTransient<TalentsController>();

string httpPort = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(int.Parse(httpPort)); });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error/500");
}

app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"),
	appBranch => appBranch.UseStatusCodePagesWithReExecute("/error/{0}")
);

app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
	var path = ctx.Request.Path.Value ?? "";

	if (path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase))
	{
		await next();
		return;
	}

	string? canonical = path switch
	{
		var p when p.StartsWith("/holy/", StringComparison.Ordinal) =>
			"/Holy/" + p["/holy/".Length..],
		var p when p.Equals("/holy", StringComparison.Ordinal) =>
			"/Holy",

		var p when p.StartsWith("/protection/", StringComparison.Ordinal) =>
			"/Protection/" + p["/protection/".Length..],
		var p when p.Equals("/protection", StringComparison.Ordinal) =>
			"/Protection",

		var p when p.StartsWith("/retribution/", StringComparison.Ordinal) =>
			"/Retribution/" + p["/retribution/".Length..],
		var p when p.Equals("/retribution", StringComparison.Ordinal) =>
			"/Retribution",

		_ => null
	};

	if (canonical is not null)
	{
		ctx.Response.Redirect(canonical + ctx.Request.QueryString.ToUriComponent(), permanent: false);
		return;
	}

	await next();
});

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Database}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Home}/{id?}"
);

// 👉 Talents pages route (Holy/Protection/Retribution → TalentsController.SectionPage)
app.MapControllerRoute(
	name: "talents-section",
	pattern: "{section:regex(^Holy|Protection|Retribution$)}/Talents",
	defaults: new { controller = "Talents", action = "SectionPage" }
);

app.MapGet("/", () => Results.Redirect("/Home/Home"));

app.MapControllerRoute(
	name: "paladin-pages",
	pattern: "{section:regex(^Holy|Protection|Retribution$)}/{slug:regex(^(?!Overview$|Gear$|Talents$|Consumables$|Rotation$|Stats$).+)}",
	defaults: new { controller = "Paladin", action = "Page" }
);

if (Environment.GetEnvironmentVariable("APPLY_MIGRATIONS_ON_STARTUP") == "true")
{
	using var scope = app.Services.CreateScope();
	try
	{
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		await db.Database.MigrateAsync();

		var seeders = scope.ServiceProvider.GetServices<ISeeder>()
			.OrderBy(s => s is UsersSeeder ? 0 :
						  s is ProductsSeeder ? 1 :
						  s is SpellbookSeeder ? 2 :
						  s is ItemsSeeder ? 3 :
						  s is DiscussionsSeeder ? 4 : 99);

		foreach (var seeder in seeders)
			await seeder.SeedAsync();
	}
	catch (Exception ex)
	{
		Console.WriteLine($"\n\n{ex.InnerException}  {ex.Message}  {ex}\n\n");
	}
}

app.Run();
