using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models
{
	public enum ProductSortBy
	{
		Relevance = 0, 
		Name,
		Price,
		Newest
	}

	public class ProductQueryOptions
	{
		[MaxLength(100)]
		public string? Search { get; set; }

		public List<string>? Categories { get; set; } = new();

		[Range(0, double.MaxValue)]
		public decimal? MinPrice { get; set; }

		[Range(0, double.MaxValue)]
		public decimal? MaxPrice { get; set; }

		public ProductSortBy SortBy { get; set; } = ProductSortBy.Relevance;
		public bool Desc { get; set; } = false;

		[Range(1, int.MaxValue)]
		public int Page { get; set; } = 1;

		[Range(1, 200)]
		public int PageSize { get; set; } = 40; 
	}
}
