using PaladinHub.Data.Models;
using PaladinHub.Data.Entities;

namespace PaladinHub.Services.Roles
{
	public interface IRoleService
	{
		Task<bool> AddUserToRole(User user, string roleName);
		Task<bool> CreateRole(string name);

	}
}