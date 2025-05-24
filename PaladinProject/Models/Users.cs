using Microsoft.AspNetCore.Identity;

namespace PaladinProject.Models
{
	public class Users : IdentityUser
	{
		public required string FullName { get; set; }
	}
}
