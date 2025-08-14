using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;
using System.Collections.Generic;

namespace PaladinHub.Models
{
	public class CombinedViewModel
	{
		// Данни от базата (за линкове/иконки)
		public List<Spell> Spells { get; set; } = new();
		public List<Item> Items { get; set; } = new();

		// Базова мета за страниците
		public string PageTitle { get; set; } = string.Empty;
		public string? PageText { get; set; }
		public string? CoverImage { get; set; }

		// Навигация
		public List<NavButton> CurrentSectionButtons { get; set; } = new();
		public List<NavButton> OtherSectionButtons { get; set; } = new();

		// Талант дървета – пълни се от ITalentTreeService
		public Dictionary<string, TalentTreeViewModel> TalentTrees { get; set; } = new();

		// По избор (ако ги ползваш)
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
