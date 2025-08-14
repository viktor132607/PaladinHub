using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;

namespace PaladinHub.Models
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

		public Dictionary<string, TalentTreeViewModel> TalentTrees { get; set; } = new();

		public List<BreadcrumbItem> Breadcrumb { get; set; } = new();
		public string? Section { get; set; }
		public string? PageHeaderTitle { get; set; }
		public string? PageHeaderSubtitle { get; set; }
		public string? PageHeaderImage { get; set; }
		public string? PageHeaderVideo { get; set; }
		public string? PageHeaderVideoPoster { get; set; }
		public List<string> PageHeaderBadges { get; set; } = new();
		public string? PageHeaderCtaText { get; set; }
		public string? PageHeaderCtaUrl { get; set; }
	}

	public class BreadcrumbItem
	{
		public string Text { get; set; } = string.Empty;
		public string Url { get; set; } = "#";
	}
}
