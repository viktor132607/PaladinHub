namespace PaladinHub.Data.Entities
{
	public class Item
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Icon { get; set; }
		public string? SecondIcon { get; set; }
		public string? Description { get; set; }

		public string? Url { get; set; }

		public int? ItemLevel { get; set; }
		public int? RequiredLevel { get; set; }
		public string? Quality { get; set; }

		//// Usability and effects
		//public string? UseEffect { get; set; }
		//public string? Duration { get; set; }
		//public bool PersistsThroughDeath { get; set; }

		//// Stack and economy
		//public int MaxStack { get; set; }
		//public int SellPriceGold { get; set; }
		//public int SellPriceSilver { get; set; }
		//public int AuctionHousePrice { get; set; }

		//// Categorization
		//public string? Category { get; set; }
		//public string? Faction { get; set; }

		//// Patch and appearance
		//public string? PatchVersion { get; set; }
		//public string? IconUrl { get; set; }
		//public string? ScreenshotUrl { get; set; }
		//public string? VideoEmbedUrl { get; set; }

		//// Optional display helpers
		//public string Tooltip => $"{Name} (iLvl {ItemLevel}) - {UseEffect}";
		//public string PriceDisplay => $"{SellPriceGold}g {SellPriceSilver}s";


	}
}
