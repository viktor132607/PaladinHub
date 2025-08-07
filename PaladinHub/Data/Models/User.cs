using Microsoft.AspNetCore.Identity;
using PaladinHub.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Entities
{
	public class User : IdentityUser
	{
		public required string FullName { get; set; }

		[Required]
		public string CartId { get; set; }

		[Required]
		public Cart Cart { get; set; }
	}
}
