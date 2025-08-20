namespace PaladinHub.Models.Products
{
	public class MerchandisePageViewModel
	{
		public ProductQueryOptions Query { get; set; } = new();
		public PagedResult<ProductViewModel> Products { get; set; } = new();
		public List<string> AllCategories { get; set; } = new();
	}
}
