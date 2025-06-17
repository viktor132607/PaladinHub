using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;

public class itemsController : Controller
{
	public List<Item> items = new List<Item>()
	{
		new Item { Id =  1, Name = "Aureate Sentry's Pledge",				 Icon = "inv_plate_raidpaladingoblin_d_01_helm.jpg",							 Description = "desc" },
		new Item { Id =  2, Name = "Strapped Rescue-Keg",					 Icon = "StrappedRescue-keg.png",												 Description = "desc" },
		new Item { Id =  3, Name = "Aureate Sentry's Roaring Will",			 Icon = "inv_plate_raidpaladingoblin_d_01_shoulder.jpg",						 Description = "desc" },
		new Item { Id =  4, Name = "Test Pilot's Go-Pack",					 Icon = "Test Pilot's Go-Pack.jpg",												 Description = "desc" },
		new Item { Id =  5, Name = "Aureate Sentry's Encasement",			 Icon = "inv_plate_raidpaladingoblin_d_01_chest.jpg",							 Description = "desc" },
		new Item { Id =  6, Name = "Revved-Up Vambraces",					 Icon = "Revved-Up Vambraces.jpg",												 Description = "desc" },
		new Item { Id =  7, Name = "Jumpstarter's Scaffold-Scrapers",		 Icon = "inv_plate_outdoorundermine_c_01_glove.jpg",							 Description = "desc" },
		new Item { Id =  8, Name = "Durable Information Securing Container", Icon = "durable_information_securing_container",								 Description = "desc" },
		new Item { Id =  9, Name = "Aureate Sentry's Legguards",			 Icon = "inv_plate_raidpaladingoblin_d_01_pant.jpg",							 Description = "desc" },
		new Item { Id = 10, Name = "Rik's Walkin' Boots",					 Icon = "Rik's Walkin' Boots.jpg",												 Description = "desc" },
		new Item { Id = 11, Name = "The Jastor Diamond",					 Icon = "TheJastorDiamond.png",													 Description = "desc" },
		new Item { Id = 12, Name = "Miniature Roulette Wheel",				 Icon = "MiniatureRouletteWheel.png",											 Description = "desc" },
		new Item { Id = 13, Name = "Eye of Kezan",							 Icon = "EyeOfKezan.png",														 Description = "desc" },
		new Item { Id = 14, Name = "Mister Pick-Me-Up",						 Icon = "Misterpick-me-up.png",													 Description = "desc" },
		new Item { Id = 15, Name = "Big Earner's Bludgeon",					 Icon = "BigEarner'sBludgeon.png",												 Description = "desc" },
		new Item { Id = 16, Name = "Titan of Industry",						 Icon = "TitanOfIndustry.png",													 Description = "desc" },
		new Item { Id = 17, Name = "Gobfather's Gifted Bling",				 Icon = "Gobfather'sGiftedBling.png",											 Description = "desc" },
		new Item { Id = 18, Name = "Dumpmech Compactors",					 Icon = "inv_glove_plate_raidwarriorgoblin_d_01.jpg",							 Description = "desc" },
		new Item { Id = 19, Name = "Coin-Operated Girdle",					 Icon = "inv_plate_raidpaladingoblin_d_01_belt.jpg",							 Description = "desc" },
		new Item { Id = 20, Name = "Cloak of Questionable Intent",			 Icon = "inv_bracer_plate_earthendungeon_c_01.jpg",								 Description = "desc" },
		new Item { Id = 21, Name = "Slashproof Business Plate",				 Icon = "inv_chest_plate_earthendungeon_c_01.jpg",								 Description = "desc" },
		new Item { Id = 22, Name = "Fuzzy Cindercuffs",						 Icon = "inv_bracer_plate_earthendungeon_c_01.jpg",								 Description = "desc" },
		new Item { Id = 23, Name = "Aureate Sentry's Gauntlets",			 Icon = "inv_plate_raidpaladingoblin_d_01_glove.jpg",							 Description = "desc" },
		new Item { Id = 24, Name = "Venture Contractor's Floodlight",		 Icon = "Venture Contractor's Floodlight.jpg",									 Description = "desc" },
		new Item { Id = 25, Name = "Sabatons of Rampaging Elements",		 Icon = "inv_boot_plate_zandalardungeon_c_01.jpg",								 Description = "desc" },
		new Item { Id = 26, Name = "Wick's Golden Loop",					 Icon = "inv_11_0_earthen_earthenring_color2.jpg",								 Description = "desc" },
		new Item { Id = 27, Name = "Bloodoath Signet",						 Icon = "inv_ring_maldraxxus_01_red.jpg",										 Description = "desc" },
		new Item { Id = 28, Name = "Signet of the Priory",					 Icon = "Signetofthepriory.png",												 Description = "desc" },
		new Item { Id = 29, Name = "Carved Blazikon Wax",					 Icon = "CarvedBlazikonWax.jpg",												 Description = "desc" },
		new Item { Id = 30, Name = "Electrifying Cognitive Amplifier",		 Icon = "inv_mace_1h_enprofession_c_01.jpg",									 Description = "desc" },
		new Item { Id = 31, Name = "Galebreaker Bulwark",					 Icon = "inv_shield_1h_earthendungeon_c_02.jpg",								 Description = "desc" },
		new Item { Id = 32, Name = "Durable Information Securing Container", Icon = "durable_information_securing_container",								 Description = "desc" },
		new Item { Id = 33, Name = "Mug's Movie Jug",						 Icon = "Mug'sMoxieJug.jpg",													 Description = "desc" },
		new Item { Id = 34, Name = "Reverb Radio",							 Icon = "Reverbradio.png",														 Description = "desc" },
		new Item { Id = 35, Name = "Greater Rune of the Void Ritual",		 Icon = "inv_inscription_majorglyph20.jpg",										 Description = "desc" },
		new Item { Id = 36, Name = "Cyrce's Circlet",						 Icon = "Cyrce's Circlet.jpg",													 Description = "desc" },
		new Item { Id = 37, Name = "Stormbringer's Runed Citrine",			 Icon = "stormbringer-icon.jpg",												 Description = "desc" },
		new Item { Id = 38, Name = "Fathomdweller's Runed Citrine",			 Icon = "fathomdweller-icon.jpg",												 Description = "desc" },
		new Item { Id = 39, Name = "Windsinger's Runed Citrine",			 Icon = "windsinger-icon.jpg",													 Description = "desc" },
		new Item { Id = 40, Name = "Soulletting Ruby",						 Icon = "SoullettingRuby.jpg",													 Description = "desc" },
		new Item { Id = 41, Name = "Bursting Lightshard",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 42, Name = "House of Cards",						 Icon = "HouseOfCards.jpg",														 Description = "desc" },
		new Item { Id = 43, Name = "Entropic Skardyn Core",					 Icon = "Entropic Skardyn Core.jpg",											 Description = "desc" },
		new Item { Id = 44, Name = "Gallagio Bottle Service",				 Icon = "GallagioBottleService.jpg",											 Description = "desc" },
		new Item { Id = 45, Name = "Unstable Power Suit Core",				 Icon = "Unstable Power Suit Core.jpg",											 Description = "desc" },
		new Item { Id = 46, Name = "Funhouse Lens",							 Icon = "Funhouse Lens.png",													 Description = "desc" },
		new Item { Id = 47, Name = "Ingenious Mana Battery",				 Icon = "Ingenious Mana Battery.jpg",											 Description = "desc" },
		new Item { Id = 48, Name = "Remnant of Darkness",					 Icon = "Remnant of Darkness.jpg",												 Description = "desc" },
		new Item { Id = 49, Name = "Vial of Spectral Essence",				 Icon = "Vial of Spectral Essence.jpg",											 Description = "desc" },
		new Item { Id = 50, Name = "Sigil of Algari Concordance",			 Icon = "Sigil of Algari Concordance.jpg",										 Description = "desc" },
		new Item { Id = 51, Name = "Darkfuse Medichopper",					 Icon = "Darkfuse Medichopper.jpg",												 Description = "desc" },
		new Item { Id = 52, Name = "Burin of the Candle King",				 Icon = "Burin of the Candle King.jpg",											 Description = "desc" },
		new Item { Id = 53, Name = "Synergistic Brewterializer",			 Icon = "Synergistic Brewterializer.jpg",										 Description = "desc" },
		new Item { Id = 54, Name = "Darkmoon Sigil: Ascension",				 Icon = "DarkmoonSigilAscension.jpg",											 Description = "desc" },
		new Item { Id = 55, Name = "Writhing Armor Banding",				 Icon = "inv_10_skinning_craftedoptionalreagent_studdedleatherswatch_color2.jpg",Description = "desc" },
		new Item { Id = 56, Name = "Dawnthread Lining",						 Icon = "inv_10_tailoring_tailoringconsumable_color2.jpg",						 Description = "desc" },
		new Item { Id = 57, Name = "Algari Alchemist Stone",				 Icon = "AlgarialchemistStone.png",												 Description = "desc" },
		new Item { Id = 58, Name = "Charged Hexsword",						 Icon = "inv_sword_1h_arathor_c_01.jpg",										 Description = "desc" },
		new Item { Id = 59, Name = "Algari Missive of the Feverflare",		 Icon = "inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg",			 Description = "desc" },
		new Item { Id = 60, Name = "Algari Missive of the Fireflash",		 Icon = "inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg",			 Description = "desc" },
		new Item { Id = 61, Name = "Waxsteel Greathelm",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 62, Name = "Consecrated Cloak",						 Icon = "default",																 Description = "desc" },
		new Item { Id = 63, Name = "Everforged Vambraces",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 64, Name = "Footbomb Championship Ring",			 Icon = "default",																 Description = "desc" },
		new Item { Id = 65, Name = "Rail Rider's Bisector",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 66, Name = "Fullthrottle Facerig",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 67, Name = "Semi-Charmed Amulet",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 68, Name = "Faded Championship Ring",				 Icon = "default",																 Description = "desc" },
		new Item { Id = 69, Name = "Remixed Ignition Saber",				 Icon = "default",																 Description = "desc" },
		new Item { Id = 70, Name = "Improvised Seaforium Pacemaker",		 Icon = "default",																 Description = "desc" },
		new Item { Id = 71, Name = "Tome of Light's Devotion",				 Icon = "default",																 Description = "desc" },
		new Item { Id = 72, Name = "Chromebustible Bomb Suit",				 Icon = "default",																 Description = "desc" },
		new Item { Id = 73, Name = "Fulgent Scintillation Shoulders",		 Icon = "default",																 Description = "desc" },
		new Item { Id = 74, Name = "Semi-Charmed Amulet",					 Icon = "default",																 Description = "desc" },
		new Item { Id = 75, Name = "Faded Championship Ring",				 Icon = "default",																 Description = "desc" },
		new Item { Id = 76, Name = "Remixed Ignition Saber",				 Icon = "default",																 Description = "desc" },
	};

	public IActionResult Details(int id)
	{
		var item = items.FirstOrDefault(i => i.Id == id);
		if (item == null)
		{
			return NotFound(); // optional: handle not found cases
		}
		return View(item);
	}
}
