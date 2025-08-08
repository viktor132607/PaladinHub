using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Data
{
	public class DataSeeder
	{
		private readonly IServiceProvider _sp;
		public DataSeeder(IServiceProvider serviceProvider) => _sp = serviceProvider;

		public async Task SeedAsync()
		{
			using var scope = _sp.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

			await db.Database.MigrateAsync();

			// 1) Роли
			string[] roles = { "User", "Admin" };
			foreach (var role in roles)
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new IdentityRole(role));

			// 2) Направи твоя акаунт Admin (ако вече съществува)
			var adminEmail = "iliev132607@gmail.com";
			var admin = await userManager.FindByEmailAsync(adminEmail);
			if (admin != null && !await userManager.IsInRoleAsync(admin, "Admin"))
				await userManager.AddToRoleAsync(admin, "Admin");

			// 3) Продукти (добавя само ако липсват по Name)
			var productsToSeed = new[]
			{
				new Product("Health Potion", 9.99m),
				new Product("Mana Potion",   8.49m),
				new Product("Steel Sword",  49.90m),
				new Product("Leather Armor", 79.00m),
			};

			foreach (var p in productsToSeed)
				if (!await db.Products.AnyAsync(x => x.Name == p.Name))
					await db.Products.AddAsync(p);

			await db.SaveChangesAsync();
		}
	}
}
