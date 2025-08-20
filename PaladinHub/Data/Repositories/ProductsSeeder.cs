using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Models;
using PaladinHub.Data.Repositories.Contracts;

namespace PaladinHub.Data
{
	public class ProductsSeeder : ISeeder
	{
		private readonly IServiceProvider _sp;
		public ProductsSeeder(IServiceProvider sp) => _sp = sp;

		public async Task SeedAsync()
		{
			using var scope = _sp.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			if (await db.Products.AnyAsync()) return;

			string[] imageUrls =
			{
				"https://cdn.ozone.bg/media/catalog/product/w/a/warcraft_the_sunwell_trilogy_ghostlands_vol_3_1670859497_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/a/warcraft_legends_vol_5_1670860569_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/a/warcraft-the-official-movie-novelization.jpg",
				"https://cdn.ozone.bg/media/catalog/product/k/u/kutiya_za_sahranenie_nemesis_now_games_world_of_warcraft_treasure_chest_13_cm_1715777312_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/h/a/halba_nemesis_now_games_world_of_warcraft_the_lich_king_1739181161_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/s/t/statuetka_mcfarlane_games_world_of_warcraft_dwarf_hunter_beast_master_marksman_15_cm_1743583319_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/s/t/statuetka_mcfarlane_games_world_of_warcraft_pandaren_monk_rogue_15_cm_1743583317_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_exploring_azeroth_the_complete_collection_1738238133_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_the_official_tarot_deck_and_guidebook_78_cards_and_booklet_1744201049_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/a/warcraft_the_sunwell_trilogy_ghostlands_vol_3_1670859497_0.jpg",
				"https://m.media-amazon.com/images/I/81rdW0NlkDL._SY522_.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_exploring_azeroth_kalimdor_1636632329_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world-of-warcraft-arthas-rise-of-the-lich-king.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_exploring_azeroth_northrend_1667388674_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_sylvanas_paperback_1678190113_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_the_dragonflight_codex_1699350680_0.jpg",
				"https://cdn.ozone.bg/media/catalog/product/w/o/world_of_warcraft_the_voices_within_short_story_collection_1739779497_0.jpg"
			};

			string[] categories =
			{
				"World of Warcraft",
				"Hearthstone",
				"Diablo II: Resurrected",
				"Diablo III",
				"StarCraft II",
				"StarCraft Remastered",
			};

			var productNamesAndPrices = new (string Name, decimal Price)[]
			{
				("World of Warcraft Chronicle Volume 4", 49.99m),
				("World of Warcraft: Sylvanas (Paperback)", 22.99m),
				("Warcraft Archive", 34.50m),
				("World of Warcraft: Arthas – Rise of the Lich King", 29.99m),
				("World of Warcraft: Illidan", 32.99m),
				("Warcraft: Durotan", 27.50m),
				("Warcraft: Lord of the Clans", 31.75m),
				("Warcraft: Day of the Dragon", 28.20m),
				("Warcraft: Tides of Darkness", 26.40m),
				("World of Warcraft: Shadows Rising", 33.60m),
				("World of Warcraft: Exploring Azeroth – The Eastern Kingdoms", 57.90m),
				("World of Warcraft: Exploring Azeroth – Kalimdor", 59.90m),
				("World of Warcraft: Exploring Azeroth – Northrend", 61.50m),
				("World of Warcraft: Exploring Azeroth – Pandaria", 62.00m),
				("World of Warcraft: Exploring Azeroth – The Dragon Isles", 63.00m),
				("World of Warcraft: The Dragonflight Codex", 66.00m),
				("World of Warcraft: War of the Scaleborn", 20.90m),
				("World of Warcraft: The Voices Within (Short Story)", 12.90m),
				("Warcraft: The Sunwell Trilogy – Dragon Hunt (Vol. 1)", 24.00m),
				("Warcraft: The Sunwell Trilogy – Shadows of Ice (Vol. 2)", 24.00m),
				("Warcraft: The Sunwell Trilogy – Ghostlands (Vol. 3)", 24.00m),
				("Warcraft: Legends, Vol. 1", 22.90m),
				("Warcraft: Legends, Vol. 2", 22.90m),
				("Warcraft: Legends, Vol. 3", 22.90m),
				("Warcraft: Legends, Vol. 4", 22.90m),
				("Warcraft: Legends, Vol. 5", 22.90m),
				("World of Warcraft: The Official Tarot Deck and Guidebook", 64.00m),
				("World of Warcraft: Exploring Azeroth (Box Set)", 309.00m),
				("Warcraft: The Official Movie Novelization", 18.00m),
				("Illidan Stormrage – Premium Statue 60 cm", 999.00m),
				("Funko POP! Games: Thrall #1046", 34.99m),
				("Funko POP! Games: Sylvanas", 34.99m),
				("World of Warcraft Hoodie – Alliance", 59.99m),
				("World of Warcraft Hoodie – Horde", 59.99m)
			};

			var products = productNamesAndPrices
				.Select((p, i) => new Product(p.Name, p.Price)
				{
					ImageUrl = imageUrls[i % imageUrls.Length],
					Category = categories[i % categories.Length]
				})
				.ToArray();

			await db.Products.AddRangeAsync(products);
			await db.SaveChangesAsync();
		}
	}
}
