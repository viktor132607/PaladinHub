using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaladinHub.Models.Products
{
	/// <summary>
	/// ViewModel за редактиране на продукт с галерия и избор на thumbnail.
	/// </summary>
	public class EditProductViewModel
	{
		[Required]
		public string Id { get; set; } = default!;

		[Required, MaxLength(100)]
		public string Name { get; set; } = default!;

		[Range(0, 1_000_000)]
		public decimal Price { get; set; }

		[MaxLength(50)]
		public string Category { get; set; } = "Other";

		[MaxLength(50)]
		public string? NewCategory { get; set; }

		[MaxLength(1000)]
		public string? Description { get; set; }

		/// <summary>
		/// Списък с всички изображения на продукта.
		/// </summary>
		public List<ProductImageInputModel> Images { get; set; } = new();

		/// <summary>
		/// FK към избраното thumbnail изображение.
		/// </summary>
		public int? ThumbnailImageId { get; set; }

		/// <summary>
		/// Индекс в Images на избраното thumbnail изображение.
		/// </summary>
		public int? ThumbnailIndex { get; set; }

		/// <summary>
		/// Dropdown за избор на категория.
		/// </summary>
		public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
	}
}
