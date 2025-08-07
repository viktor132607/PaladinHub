using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Models.Carts
{
	public class CartViewModel
	{
		public string Id { get; set; }
		public string OrderDate { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }
		public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
		public ICollection<Product> Products { get; set; } = new List<Product>();

	}
}