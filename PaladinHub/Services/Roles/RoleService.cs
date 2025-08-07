using Microsoft.AspNetCore.Identity;
using PaladinHub.Data.Entities;
using PaladinHub.Services.Roles;

namespace OnlineFoodShop.Services.Roles
{
	public class RoleService : IRoleService
	{
		private RoleManager<IdentityRole> roleManager;
		private UserManager<User> userManager;

		public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}
		public async Task<bool> CreateRole(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}
			await roleManager.CreateAsync(new IdentityRole
			{
				Name = name
			});
			return true;
		}
		public async Task<bool> AddUserToRole(User user, string roleName)
		{
			if (string.IsNullOrWhiteSpace(roleName) || user is null)
			{
				return false;
			}
			await userManager.AddToRoleAsync(user, roleName);
			return true;
		}
	}
}