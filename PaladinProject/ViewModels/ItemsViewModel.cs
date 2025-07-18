using PaladinProject.Models;

namespace PaladinProject.ViewModels
{
	public class ItemsViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Icon { get; set; }
		public string? Description { get; set; }
		public string? Irl { get; set; }
		public int? ItemLevel { get; set; }
		public int? RequiredLevel { get; set; }
		public string? Quality { get; set; }
	}
}
