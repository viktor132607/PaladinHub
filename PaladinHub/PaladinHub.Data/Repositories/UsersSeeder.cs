using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Repositories.Contracts;

namespace PaladinHub.Data
{
	public class UsersSeeder : ISeeder
	{
		private readonly IServiceProvider _sp;
		public UsersSeeder(IServiceProvider sp) => _sp = sp;

		public async Task SeedAsync()
		{
			using var scope = _sp.CreateScope();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

			foreach (var role in new[] { "Admin", "User" })
			{
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new IdentityRole(role));
			}

			var adminName = "Viktor Iliev";
			var adminEmail = "iliev132607@gmail.com";
			var adminPassword = "iliev132607@gmail.com";

			var admin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
			if (admin is null)
			{
				admin = new User
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true,
					FullName = adminName
				};
				var result = await userManager.CreateAsync(admin, adminPassword);
				if (!result.Succeeded)
				{
					var msg = string.Join("; ", result.Errors.Select(e => e.Description));
					throw new Exception("Admin create failed: " + msg);
				}
			}
			if (!await userManager.IsInRoleAsync(admin, "Admin"))
				await userManager.AddToRoleAsync(admin, "Admin");

			var defaultUserPassword = "Paladin#12345"; 
			var demoUsers = new (string FullName, string Email)[]
			{
				("Mila Petkova",      "mila.petkova@paladinhub.test"),
				("Ivan Dimitrov",     "ivan.dimitrov@paladinhub.test"),
				("Georgi Stoyanov",   "georgi.stoyanov@paladinhub.test"),
				("Elena Nikolova",    "elena.nikolova@paladinhub.test"),
				("Petar Ivanov",      "petar.ivanov@paladinhub.test"),
				("Raya Koleva",       "raya.koleva@paladinhub.test"),
				("Dimitar Angelov",   "dimitar.angelov@paladinhub.test"),
				("Vesela Marinova",   "vesela.marinova@paladinhub.test"),
				("Kalin Hristov",     "kalin.hristov@paladinhub.test"),
				("Sofia Georgieva",   "sofia.georgieva@paladinhub.test"),
			};

			foreach (var (name, email) in demoUsers)
			{
				var user = await userManager.FindByEmailAsync(email);
				if (user is null)
				{
					user = new User
					{
						UserName = email,
						Email = email,
						EmailConfirmed = true,
						FullName = name
					};
					var createRes = await userManager.CreateAsync(user, defaultUserPassword);
					if (!createRes.Succeeded)
					{
						var msg = string.Join("; ", createRes.Errors.Select(e => e.Description));
						throw new Exception($"User create failed ({email}): " + msg);
					}
				}

				if (!await userManager.IsInRoleAsync(user, "User"))
					await userManager.AddToRoleAsync(user, "User");
			}
		}
	}
}
