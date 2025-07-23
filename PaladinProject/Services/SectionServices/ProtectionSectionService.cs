using PaladinProject.Services.IService;
using PaladinProject.ViewModels;

namespace PaladinProject.Services.SectionServices
{
	public class ProtectionSectionService : BaseSectionService
	{
		public override string ControllerName => "Protection";

		public override string GetCoverImage() => "/images/ProtCoverV5.png";

		public override string GetPageTitle(string actionName) => actionName switch
		{
			"Overview" => "Protection Paladin Overview",
			"Talents" => "Best Talents for Protection",
			"Stats" => "Stat Priorities for Protection",
			"Consumables" => "Protection Consumables Guide",
			"Gear" => "Best-in-Slot Protection Gear",
			"Rotation" => "Protection Paladin Rotation",
			_ => "Protection Paladin"
		};

		public override string? GetPageText(string actionName) => actionName switch
		{
			"Overview" => "A tank specialized in shields, mitigation, and damage reduction.",
			"Talents" => "Top protection talents for survivability and control.",
			"Gear" => "Gear choices to maximize survivability and threat.",
			"Consumables" => "Use these consumables to stay alive and boost defenses.",
			"Stats" => "Stat priority focused on armor, stamina, and block.",
			"Rotation" => "Taunt, shield slam, and keep mitigation up.",
			_ => null
		};
	}
}
