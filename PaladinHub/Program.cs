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

Env.Load(); // Зареждаме .env файла

var builder = WebApplication.CreateBuilder(args);

// MVC + глобални филтри/локации
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

// Сървиси
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

// ***** Page Builder услуги *****
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IJsonLayoutValidator, JsonLayoutValidator>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IDataPresetService, DataPresetService>();

// *** DI за HTML Sanitizer ***
builder.Services.AddSingleton<IHtmlSanitizer>(_ =>
{
	var s = new HtmlSanitizer();
	s.AllowedTags.Add("p"); s.AllowedTags.Add("b"); s.AllowedTags.Add("i");
	s.AllowedTags.Add("ul"); s.AllowedTags.Add("ol"); s.AllowedTags.Add("li");
	s.AllowedTags.Add("a"); s.AllowedAttributes.Add("href");
	return s;
});

// case-insensitive секции: {section:palsec}
builder.Services.Configure<RouteOptions>(opt =>
{
	opt.ConstraintMap["palsec"] = typeof(AllowedSectionConstraint);
});

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
	string conn = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
	options.UseNpgsql(conn);
});

// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT Auth - четем от .env
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

// Redis connection & CartStore
string redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
builder.Services.AddScoped<ICartStore, RedisCartStore>();

// *** Talents orchestrator DI (НЕ е MVC контролер вече) ***
builder.Services.AddTransient<TalentsController>();     // <-- важно за оркестратора

// Kestrel Port (за контейнер/хостинг)
string httpPort = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(int.Parse(httpPort)); });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error/500");
}

// Не прилагай status page re-execute за /api (чупи POST-ове от fetch)
app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"),
	appBranch => appBranch.UseStatusCodePagesWithReExecute("/error/{0}")
);

app.UseStaticFiles();

// Канонизация на адресите към главна буква (без Admin); некеширащ redirect (permanent:false)
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

// ВАЖНО: Authentication преди Authorization
app.UseAuthentication();
app.UseAuthorization();

// Attribute-routed контролери
app.MapControllers();

// Areas
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Database}/{action=Index}/{id?}"
);

// Статичните MVC маршрути (default към Home/Home)
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Home}/{id?}"
);

// Изричен root -> Home/Home
app.MapGet("/", () => Results.Redirect("/Home/Home"));

// Динамичните CMS страници – /{Section}/{slug}
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
