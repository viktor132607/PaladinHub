namespace PaladinProject.Models
{
	public class Spell
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Icon { get; set; }
		public string? Description { get; set; }
		public string? Url { get; set; }

		private string _quality = "spell";
		public string Quality
		{
			get => _quality;
			set => _quality = string.IsNullOrWhiteSpace(value) ? "spell" : value.ToLowerInvariant();

			//public int X { get; set; }
			//public int Y { get; set; }

			// basic item metadata
			//public int itemlevel { get; set; }
			//public int requiredlevel { get; set; }
			//public string? quality { get; set; }

			//// usability and effects
			//public string? useeffect { get; set; }
			//public string? duration { get; set; }
			//public bool persiststhroughdeath { get; set; }

			//// stack and economy
			//public int maxstack { get; set; }
			//public int sellpricegold { get; set; }
			//public int sellpricesilver { get; set; }
			//public int auctionhouseprice { get; set; }

			//// categorization
			//public string? category { get; set; }
			//public string? faction { get; set; }

			//// patch and appearance
			//public string? patchversion { get; set; }
			//public string? iconurl { get; set; }
			//public string? screenshoturl { get; set; }
			//public string? videoembedurl { get; set; }

			// optional display helpers
			//public string tooltip => $"{name} (ilvl {itemlevel}) - {useeffect}";
			//public string pricedisplay => $"{sellpricegold}g {sellpricesilver}s";
		}
	}
}
