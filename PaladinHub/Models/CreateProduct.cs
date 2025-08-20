using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models.Products
{
	public class CreateProductViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		[Url]
		public string ImageUrl { get; set; }

		[Required]
		[MaxLength(50)]
		public string Category { get; set; }

		[MaxLength(1000)]
		public string? Description { get; set; }


	}
}
