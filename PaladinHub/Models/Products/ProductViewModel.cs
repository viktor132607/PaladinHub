using PaladinHub.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models.Products
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

		// ⭐ Ново поле за среден рейтинг
		[Range(0, 5)]
		public double AverageRating { get; set; }

		// ⭐ Ново поле за брой ревюта
		public int ReviewsCount { get; set; }
	}
}
