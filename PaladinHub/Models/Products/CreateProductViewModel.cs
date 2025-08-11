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
		public string? ImageUrl { get; set; }

		[MaxLength(100)]
		public string? Category { get; set; }

		[MaxLength(100)]
		public string? NewCategory { get; set; }

		[MaxLength(2000)]
		public string? Description { get; set; }

		public IEnumerable<SelectListItem> CategorySelectList { get; set; }
			= System.Linq.Enumerable.Empty<SelectListItem>();

		public List<AddProductImageInput> Images { get; set; } = new();
	}

	public class AddProductImageInput
	{
		[Url, MaxLength(2048)]
		public string? Url { get; set; }
		public int SortOrder { get; set; }
	}
}
