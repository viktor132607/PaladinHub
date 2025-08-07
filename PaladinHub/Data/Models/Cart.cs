using PaladinHub.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class Cart
	{
		[Required]
		public string Id { get; init; } = Guid.NewGuid().ToString();
		public bool IsArchived { get; set; }
		public string? OrderDate { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }
		public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
		public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}