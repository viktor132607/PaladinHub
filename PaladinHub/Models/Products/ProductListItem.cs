namespace PaladinHub.Models.Products
{
	public class ProductListItem
	{
		public string Id { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string Category { get; set; } = default!;
		public string? ImageUrl { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }

		// ⭐ Нови полета за рейтинг
		public decimal AverageRating { get; set; } // 0..5
		public int ReviewsCount { get; set; }
	}
}
