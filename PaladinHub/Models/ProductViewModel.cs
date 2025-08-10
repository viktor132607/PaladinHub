using PaladinHub.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models
{
	public class ProductViewModel
	{
		[Required]
		public string Id { get; init; }

		[Required]
		public string Name { get; set; } = default!;

		[Required]
		public decimal Price { get; set; }

		public string? ImageUrl { get; set; } 

		public int Quantity { get; set; }

		public Guid CartId { get; set; }

		public Cart Cart { get; set; } = default!;

		public string Category { get; set; } = "Other";

		public string? Description { get; set; }

	}
}
