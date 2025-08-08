using PaladinHub.Data;
using PaladinHub.Services.Carts;
using PaladinHub.Services.IService;
using PaladinHub.Services.Products;
using PaladinHub.Services.Roles;
using PaladinHub.Services.SectionServices;

namespace PaladinHub.ServiceExtensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddCustomServices(this IServiceCollection services)
		{
			// DATA SEEDER
			services.AddScoped<DataSeeder>();

			// CORE SERVICES
			services.AddScoped<ISpellbookService, SpellbookService>();
			services.AddScoped<IItemsService, ItemsService>();

			// CART / PRODUCTS / ROLES
			services.AddScoped<ICartService, CartService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IRoleService, RoleService>();

			// SECTION SERVICES (инжектират се като конкретни класове в контролерите)
			services.AddTransient<HolySectionService>();
			services.AddTransient<ProtectionSectionService>();
			services.AddTransient<RetributionSectionService>();

			return services;
		}
	}
}
