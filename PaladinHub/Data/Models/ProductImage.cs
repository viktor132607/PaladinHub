using System;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class ProductImage
	{
		public int Id { get; set; }

		[Required] public string ProductId { get; set; } = default!;
		[Required, Url] public string Url { get; set; } = default!;

		public int SortOrder { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
