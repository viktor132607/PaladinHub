using PaladinHub.Services.IService;
using PaladinHub.Models;

namespace PaladinHub.Services.SectionServices
{
	public class RetributionSectionService : BaseSectionService
	{
		public override string ControllerName => "Retribution";

		public override string GetCoverImage() => "/images/RetributionCoverOrig.jpg";

		public override string GetPageTitle(string actionName) => actionName switch
		{
			"Overview" => "Retribution Paladin Overview",
			"Talents" => "Best Talents for Retribution",
			"Stats" => "Stat Priorities for Retribution",
			"Consumables" => "Retribution Paladin Consumables",
			"Gear" => "BiS Gear for Retribution",
			"Rotation" => "Retribution Paladin Rotation",
			_ => "Retribution Paladin"
		};

		public override string? GetPageText(string actionName) => actionName switch
		{
			"Overview" => "A melee DPS that excels in burst and utility support.",
			"Talents" => "These talents bring the best out of your burst windows.",
			"Gear" => "Recommended gear for maximum damage output.",
			"Consumables" => "Best potions and food for Retribution DPS.",
			"Stats" => "Strength-focused stat priorities for burst damage.",
			"Rotation" => "Hammer everything, then Templar’s Verdict.",
			_ => null
		};
	}
}
