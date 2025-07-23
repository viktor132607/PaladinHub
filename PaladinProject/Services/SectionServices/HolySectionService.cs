using PaladinProject.Services.SectionServices;

public class HolySectionService : BaseSectionService
{
	public override string ControllerName => "Holy";

	public override string GetCoverImage() => "/images/TheHolyCover2.jpg";

	public override string GetPageTitle(string actionName) => actionName switch
	{
		"Overview" => "Holy Paladin Overview",
		"Talents" => "Best Talents for Holy",
		"Stats" => "Stat Priorities for Holy",
		"Consumables" => "Holy Paladin Consumables",
		"Gear" => "Best-in-Slot Gear for Holy",
		"Rotation" => "Holy Paladin Rotation",
		_ => "Holy Paladin"
	};

	public override string? GetPageText(string actionName) => actionName switch
	{
		"Overview" => "A healer specialized in powerful single-target spells and blessings.",
		"Talents" => "These are the best talents for Holy Paladin across different scenarios.",
		"Gear" => "BiS list and gear progression guide for Holy Paladin.",
		"Consumables" => "Use these to stay effective in raids and dungeons.",
		"Stats" => "Stat priority and weights for maximum healing throughput.",
		"Rotation" => "Healing priorities and Holy Shock optimization.",
		_ => null
	};
}
