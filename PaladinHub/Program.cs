using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Repositories.Contracts;
using PaladinHub.Infrastructure;
using PaladinHub.Services;
using PaladinHub.Services.Discussions;
using PaladinHub.Services.SectionServices;
using PaladinHub.Services.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Зареждай .env само локално
if (builder.Environment.IsDevelopment())
{
	Env.Load();
}

// MVC + Razor
builder.Services
	.AddControllersWithViews(options => options.Filters.Add<LoadGlobalDataFilter>())
	.AddRazorOptions(o => o.ViewLocationExpanders.Add(new SectionViewLocationExpander()));

// --- Build connection string ---
static string BuildPgConnectionString()
{
	var cs = Environment.GetEnvironmentVariable("DB_CONNECTION");
	if (!string.IsNullOrWhiteSpace(cs)) return cs;

	var url = Environment.GetEnvironmentVariable("DATABASE_URL"); // fallback
	if (!string.IsNullOrWhiteSpace(url))
	{
		var u = new Uri(url);
		var ui = (u.UserInfo ?? "").Split(':', 2);
		var user = ui.Length > 0 ? Uri.UnescapeDataString(ui[0]) : "";
		var pass = ui.Length > 1 ? Uri.UnescapeDataString(ui[1]) : "";

		var b = new NpgsqlConnectionStringBuilder
		{
			Host = u.Host,
			Port = u.IsDefaultPort ? 5432 : u.Port,
			Username = user,
			Password = pass,
			Database = u.AbsolutePath.Trim('/'),
			SslMode = SslMode.Require,
			TrustServerCertificate = true,
			Pooling = true,
			MinPoolSize = 0,
			MaxPoolSize = 25,
			Timeout = 15,
			KeepAlive = 30
		};
		return b.ToString();
	}

	throw new InvalidOperationException("DB_CONNECTION/DATABASE_URL is not set.");
}

var conn = BuildPgConnectionString();

// Безопасен лог (без парола)
try
{
	var b = new NpgsqlConnectionStringBuilder(conn);
	Console.WriteLine($"[DB] Host={b.Host}; Db={b.Database}; User={b.Username}");
}
catch { }

// DbContext (Postgres) + кратки retry-та
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseNpgsql(conn, npg => npg.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null));
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

// DI на твоите услуги
builder.Services.AddCustomServices();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISpellbookService, SpellbookService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddHttpContextAccessor();

// Порт за Render
string httpPort = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(int.Parse(httpPort)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Database}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "paladin-section",
	pattern: "{section}/{action=Overview}/{id?}",
	defaults: new { controller = "Paladin" }
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Home}/{id?}"
);

// Авто-миграции + seed (включи с env APPLY_MIGRATIONS_ON_STARTUP=true)
if (Environment.GetEnvironmentVariable("APPLY_MIGRATIONS_ON_STARTUP") == "false")
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
		Console.WriteLine($"\n[DB][Migrate] {ex.GetType().Name}: {ex.Message}\n{ex}\n");
	}
}

app.Run();
