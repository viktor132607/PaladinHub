using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Services.IService;
using PaladinHub.Services.SectionServices;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISpellbookService, SpellbookService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();

builder.Services.AddHttpContextAccessor();

Console.WriteLine($"\n\n{Environment.GetEnvironmentVariable("DB_CONNECTION")}\n\n");

builder.Services
	.AddDbContext<AppDbContext>(
		options =>
			options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION")!)
	);

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

// Configure the HTTP request pipeline
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
	pattern: "{controller=Home}/{action=Home}/{id?}");


using (IServiceScope scope = app.Services.CreateScope())
{
	try
	{
		AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		await context.Database.MigrateAsync();
	}
	catch (Exception ex)
	{
		Console.WriteLine($"\n\n\n{ex.InnerException}.  {ex.Message}  {ex.ToString}\n\n\n");
	}
}

app.Run();
