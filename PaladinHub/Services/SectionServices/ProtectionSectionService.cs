using PaladinHub.Services.IService;
using PaladinHub.Models;

namespace PaladinHub.Services.SectionServices
{
	public class ProtectionSectionService : BaseSectionService
	{
		public override string ControllerName => "Protection";

		public override string GetCoverImage() => "/images/ProtCoverV5.png";

		public override string GetPageTitle(string actionName) => actionName switch
		{
			"Overview" => "Protection Paladin Tank Guide - The War Within",
			"Talents" => "Best Protection Paladin Talent Tree Builds - The War Within",
			"Stats" => "Protection Paladin Stat Priority - The War Within",
			"Consumables" => "Protection Paladin Enchants & Consumables - The War Within",
			"Gear" => "Protection Paladin Gear and Best in Slot - The War Within",
			"Rotation" => "Protection Paladin Rotation Guide - The War Within 11.1.7"
		};

		public override string GetPageText(string actionName) => actionName switch
		{
			"Overview" => "Welcome to Patch 11.1.7 & Season 2 Protection Paladin guide. This guide will help you master your Protection Paladin in all aspects of the game including raids and dungeons.",
			"Talents" => "Here are all the best Protection Paladin Talent Tree builds in the Patch 11.1.7 & Season 2 for raids and Mythic+, including export links to import these builds directly into the game.\r\n\r\nFor recommended talent builds for each raid boss and Mythic+ dungeon, check out our Liberation of Undermine Raid Page and Mythic+ page.",
			"Gear" => "Gear is one of the most important elements in WoW to strengthen your Protection Paladin, providing massive amounts of stats as well as armor, procs, and set bonuses.\r\n\r\nWe will explain how to obtain the best gear for your Protection Paladin in Patch 11.1.7 & Season 2 and how to check if a piece is BiS, an upgrade, or just bad. This guide will help you select the best pieces of gear from Dungeons and Raids in The War Within, whether they be weapons, trinkets, or armor.\r\n",
			"Consumables" => "Consumables are a vital part of high-level content in WoW, like Mythic+ Dungeons and Raids, providing additional ways for players to improve and customize their stats outside of gear.\r\n\r\nIn this guide, we will explain the best Protection Paladin gems, Protection Paladin flasks, Protection Paladin potions, and Protection Paladin enchants in Patch 11.1.7 & Season 2, as well as cheaper alternatives.\r\nBelow you will find the best Protection Paladin enchants and consumables. Make sure to also check our The War Within Profession Guide for all profession details, updated for Patch 11.1.7 & Season 2.",
			"Stats" => "Stats are a key component when customizing your Protection Paladin in World of Warcraft The War Within--having the right combination of them can be crucial to your performance.\r\n\r\nIn this guide, we will detail the best stat priority for your Protection Paladin, as well as provide explanations covering how to determine Protection Paladin stat priorities personalized for your character in Patch 11.1.7 & Season 2, as well as how to check if a piece of gear is BiS, upgrade or just bad for you.\r\n\r\nBesides talking about your Protection Paladin stat priority, we will also cover your stats in-depth, explaining nuances and synergies for niche situations that go beyond a generic Protection Paladin priority.",
			"Rotation" => "Learn the best Protection Paladin rotation for The War Within Season 2. Details about how to excel at your Protection Paladin and the optimal rotation for all talent builds in dungeons and raids for Patch 11.1.7 & Season 2."
		};
	}
}
