using PaladinProject.Models;

namespace PaladinProject.ViewModels
{
	public class CombinedViewModel
	{
		public List<Spell> Spells { get; set; } = new();
		public List<Item> Items { get; set; } = new();

		public string PageTitle { get; set; } = string.Empty;

		public string? PageText { get; set; }

		public string? CoverImage { get; set; }
		public List<NavButton> CurrentSectionButtons { get; set; } = new();
		public List<NavButton> OtherSectionButtons { get; set; } = new();
	}
}
