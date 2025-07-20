using PaladinProject.Models;

namespace PaladinProject.ViewModels
{
	public class CombinedViewModel
	{
		public List<Spell> Spells { get; set; } = new();
		public List<Item> Items { get; set; } = new();
	}
}
