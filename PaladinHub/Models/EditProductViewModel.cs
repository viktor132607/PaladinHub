using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models.Products
{
	public class EditProductViewModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required, MaxLength(100)]
		public string Name { get; set; } = null!;

		[Range(0, 999999)]
		public decimal Price { get; set; }

		[MaxLength(300)]
		public string? ImageUrl { get; set; }

		[MaxLength(50)]
		public string? Category { get; set; }

		[MaxLength(1000)]
		public string? Description { get; set; }
	}
}
