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

		public Product(string name, decimal price, string? imageUrl)
		{
			this.Name = name;
			this.Price = price;
			this.ImageUrl = imageUrl;
		}

		[Required]
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Url]
		public string? ImageUrl { get; set; }  

		public ICollection<CartProduct> Carts { get; set; } = new List<CartProduct>();

		[MaxLength(50)]
		public string Category { get; set; } = "Other";

		[MaxLength(1000)]
		public string? Description { get; set; }


	}
}
