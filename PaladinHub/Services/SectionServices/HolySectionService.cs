using PaladinHub.Services.SectionServices;

public class HolySectionService : BaseSectionService
{
	public override string ControllerName => "Holy";

	public override string GetCoverImage() => "/images/TheHolyCover2.jpg";

	public override string GetPageTitle(string actionName) => actionName switch
	{
		"Overview" => "Holy Paladin Healer Guide - The War Within",
		"Talents" => "Best Holy Paladin Talent Tree Builds - The War Within",
		"Stats" => "Holy Paladin Stat Priority - The War Within",
		"Consumables" => "Holy Paladin Enchants & Consumables - The War Within",
		"Gear" => "Holy Paladin Gear and Best in Slot - The War Within",
		"Rotation" => "Holy Paladin Rotation Guide - The War Within",
		_ => "Holy Paladin"
	};

	public override string? GetPageText(string actionName) => actionName switch
	{
		"Overview" => "\t\t\t\t\tHoly Paladin is a plate-wearing Healer specialization with a wide range of damage reduction and defensive abilities. We specialize in healing specific targets with large single-target heals, commonly referred to as “spot healing”. Holy Paladin gets access to the iconic Beacon of Light ability at level 16, which allows us to keep a consistent stream of healing on a specific target while healing other allies who might need it!\r\n\r\n\t\t\t\t\tBesides the classic Healer resource, Mana, Holy Paladins also utilize a secondary resource known as Holy Power, which functions similarly to Combo Points. Most of our spells generate this resource, which we can then use to cast our most powerful heals, Word of Glory and Light of Dawn.\r\n",
		"Talents" => "These are the best talents for Holy Paladin across different scenarios.",
		"Gear" => "\t\t\t\t\tGear is one of the most important elements in WoW to strengthen your Holy Paladin, providing massive amounts of stats as well as armor, procs, and set bonuses.\r\n\r\n\t\t\t\t\tThis guide will explain how to obtain the best gear for your Holy Paladin in Patch 11.1.5 & Season 2 and how to check if a piece is Best in Slot (BiS), an upgrade, or just bad.\r\n\t\t\t\t\tThis guide will help you select the best pieces of gear from Dungeons and Raids in The War Within, whether they be weapons, trinkets, or armor.\r\n",
		"Consumables" => "Use these to stay effective in raids and dungeons.",
		"Stats" => "Stat priority and weights for maximum healing throughput.",
		"Rotation" => "Healing priorities and Holy Shock optimization.",
		_ => null
	};
}
