using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.ServiceExtensions;
using PaladinHub.Services.Discussions;
using PaladinHub.Services.IService;
using PaladinHub.Services.SectionServices;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// твоите услуги
builder.Services.AddCustomServices();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISpellbookService, SpellbookService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();


// 👉 DataSeeder за роли/админ/продукти
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddHttpContextAccessor();

Console.WriteLine($"\n\n{Environment.GetEnvironmentVariable("DB_CONNECTION")}\n\n");

builder.Services.AddDbContext<AppDbContext>(options =>
{
	var conn = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
	options.UseNpgsql(conn);
});

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

string port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(options =>
{
	options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// ---- pipeline ----
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Home}/{id?}"
);

// 👉 миграция + seed при старт
using (var scope = app.Services.CreateScope())
{
	try
	{
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		await context.Database.MigrateAsync();

		var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
		await seeder.SeedAsync();
	}
	catch (Exception ex)
	{
		Console.WriteLine($"\n\n\n{ex.InnerException}  {ex.Message}  {ex}\n\n\n");
	}
}

app.Run();
