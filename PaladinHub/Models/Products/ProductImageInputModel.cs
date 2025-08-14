using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models.Products
{
	/// <summary>
	/// Input модел за едно изображение в галерията при Create/Edit.
	/// </summary>
	public class ProductImageInputModel
	{
		public int? Id { get; set; } // null при ново изображение

		[Required, Url, MaxLength(2048)]
		public string Url { get; set; } = default!;

		[Range(0, int.MaxValue)]
		public int SortOrder { get; set; }

		[MaxLength(300)]
		public string? AltText { get; set; }
	}
}
