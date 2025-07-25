using Microsoft.AspNetCore.Identity;

namespace PaladinHub.Data.Entities
{
	public class User : IdentityUser
	{
		public required string FullName { get; set; }
	}
}
