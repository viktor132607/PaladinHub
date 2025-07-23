using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PaladinProject.Data;
using PaladinProject.Models;
using PaladinProject.Services;
using PaladinProject.Helpers;
using PaladinProject.Services.IService;
using PaladinProject.Services.SectionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISpellbookService, SpellbookService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();
builder.Services.AddScoped<HolySectionService>();
builder.Services.AddScoped<ProtectionSectionService>();
builder.Services.AddScoped<RetributionSectionService>();




// ?? Add RazorHelpers support
builder.Services.AddHttpContextAccessor();

// Register main application database
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Register SpellbookContext
builder.Services.AddDbContext<SpellbookContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Identity configuration
builder.Services.AddIdentity<Users, IdentityRole>(options =>
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

builder.Services.AddScoped<IEmailSender, EmailSender>();

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

app.Run();
