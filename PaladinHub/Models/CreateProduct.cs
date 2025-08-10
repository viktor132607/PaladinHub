using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaladinHub.Models.Products
{
	public class CreateProductViewModel
	{
		[Required, StringLength(200)]
		public string Name { get; set; } = default!;

		[Range(0, 1_000_000)]
		public decimal Price { get; set; }

		[Url, MaxLength(2048)]
		public string? ImageUrl { get; set; }   // главна снимка (thumbnail)

		[MaxLength(100)]
		public string? Category { get; set; }   // избрана от dropdown

		[MaxLength(100)]
		public string? NewCategory { get; set; } // ако е попълнено – ползваме него

		[MaxLength(2000)]
		public string? Description { get; set; }

		public IEnumerable<SelectListItem> CategorySelectList { get; set; } = System.Linq.Enumerable.Empty<SelectListItem>();

		// Галерия от доп. снимки (optional)
		public List<AddProductImageInput> Images { get; set; } = new();
	}

	public class AddProductImageInput
	{
		[Url, MaxLength(2048)]
		public string? Url { get; set; }

		public int SortOrder { get; set; }
	}
}
