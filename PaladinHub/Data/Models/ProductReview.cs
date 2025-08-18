using System;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class ProductReview
	{
		[Key]
		public int Id { get; set; }

		[Required] public string ProductId { get; set; } = default!;
		/// <summary>Навигация назад към продукта.</summary>
		public Product Product { get; set; } = default!;

		[Required] public string UserId { get; set; } = default!;

		[Range(1, 5)] public int Rating { get; set; }

		[Required, MaxLength(2000)] public string Content { get; set; } = default!;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
