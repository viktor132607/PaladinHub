using PaladinHub.Data;
using PaladinHub.Data.Repositories.Contracts;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Products;
using PaladinHub.Services.Roles;
using PaladinHub.Services.SectionServices;

namespace PaladinHub.Services.ServiceExtension
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddCustomServices(this IServiceCollection services)
		{
			services.AddScoped<ISeeder, UsersSeeder>();
			services.AddScoped<ISeeder, ProductsSeeder>();
			services.AddScoped<ISeeder, SpellbookSeeder>();
			services.AddScoped<ISeeder, ItemsSeeder>();
			services.AddScoped<ISeeder, DiscussionsSeeder>();

			services.AddScoped<ISpellbookService, SpellbookService>();
			services.AddScoped<IItemsService, ItemsService>();

			services.AddScoped<ICartService, CartService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IRoleService, RoleService>();

			services.AddTransient<HolySectionService>();
			services.AddTransient<ProtectionSectionService>();
			services.AddTransient<RetributionSectionService>();



			return services;
		}
	}
}
