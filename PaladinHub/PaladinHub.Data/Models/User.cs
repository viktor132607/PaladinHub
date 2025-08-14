using Microsoft.AspNetCore.Identity;
using PaladinHub.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Entities
{
	public class User : IdentityUser
	{
		[Required]
		public string FullName { get; set; } = default!;

		public Guid? CartId { get; set; }

		public Cart Cart { get; set; } = default!;
	}
}
