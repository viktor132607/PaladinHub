using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaladinHub.Data.Models
{
	public class Product
	{
		public Product(string name, decimal price)
		{
			this.Name = name;
			this.Price = price;
		}
		[Required]
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		public ICollection<CartProduct> Carts { get; set; } = new List<CartProduct>();
	}
}