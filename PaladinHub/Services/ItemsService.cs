using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PaladinHub.Data.Entities;
using PaladinHub.Services.IService;
using System;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

public class ItemsService : IItemsService
{
	public List<Item> GetAllItems()
	{
		return new List<Item>
		{
			new Item { Id =1,
				Name ="Greater Rune of the Void Ritual",
				Icon = "inv_inscription_majorglyph20.jpg",
				ItemLevel = 610,
				Quality = "Epic",
				RequiredLevel = 70 ,
				Url = "https://www.wowhead.com/spell=469411/a-just-reward",
				Description = "Use: Apply Greater Rune of the Void Ritual to a helm.Gain Void Ritual, giving your spells and abilities a chance to increase all secondary stats by 82 every sec for 20 sec.\r\n\r\nCannot be applied to items lower than level 350. This effect is fleeting and will only work during The War Within Season 2."},

				new Item { Id =2,
					Name ="Enchant Weapon - Council's Guile",
					Icon = "inv_inscription_minorglyph20.jpg",
					SecondIcon = "professions-chaticon-quality-tier3.png",
					ItemLevel = 610,
					Quality = "Rare", 
					RequiredLevel = 70 ,
					Url = "https://www.wowhead.com/item=223759/enchant-weapon-councils-guile", 
					Description = "\r\nUse: Earthen Enhancements - Wondrous Weapons\r\n\r\nPermanently enchants a weapon to sometimes grant you Keen Prowess, bestowing 3910 Critical Strike to you for 12 sec. Cannot be applied to items lower than level 350."},

				new Item
				{
					Id = 3,
					Name = "Lesser Rune of the Void Ritual",
					Icon = "inv_inscription_minorglyph20.jpg",
					ItemLevel = 580,
					Quality = "Rare",
					RequiredLevel = 70,
					Url = "https://www.wowhead.com/item=212885/lesser-rune-of-the-void-ritual",
					Description = "Use: Apply Lesser Rune of the Void Ritual to a helm. Gain Void Ritual, giving your spells and abilities a chance to increase all secondary stats by 47 every sec for 20 sec.\r\n\r\nCannot be applied to items lower than level 350. This effect is fleeting and will only work during The War Within Season 2."
				},
				new Item
				{
					Id = 4,
					Name = "Flask of Tempered Swiftness",
					Icon = "inv_potion_green.jpg",
					SecondIcon = "professions-chaticon-quality-tier3.png",
					ItemLevel = 610,
					Quality = "Common",
					RequiredLevel = 71,
					Url = "https://www.wowhead.com/item=212741/flask-of-tempered-swiftness",
					Description = "Use: Drink to increase your Haste by 2825.\r\n\r\nLasts 1 hour and through death. Consuming an identical flask will add another 1 hour."
				},

				new Item
				{
					Id = 5,
					Name = "Algari Mana Potion",
					Icon = "inv_flask_blue.jpg",
					SecondIcon = "professions-chaticon-quality-tier3.png",
					ItemLevel = 610,
					Quality = "Common",
					RequiredLevel = 71,
					Url = "https://www.wowhead.com/item=212739/algari-mana-potion",
					Description = "Use: Restores 270000 mana."
				},

				new Item
				{
					Id = 6,
					Name = "Algari Healing Potion",
					Icon = "inv_flask_red.jpg",
					SecondIcon = "professions-chaticon-quality-tier3.png",
					ItemLevel = 610,
					Quality = "Common",
					RequiredLevel = 71,
					Url = "https://www.wowhead.com/item=212738/algari-healing-potion",
					Description = "Use: Restores 3839450 health."
				},

				new Item
				{
					Id = 7,
					Name = "Algari Mana Oil",
					Icon = "trade_alchemy_potiond1.jpg",
					SecondIcon = "professions-chaticon-quality-tier3.png",
					ItemLevel = 70,
					Quality = "Uncommon",
					RequiredLevel = 68,
					Url = "https://www.wowhead.com/item=212740/algari-mana-oil",
					Description = "Use: Coat your weapon in Algari Mana Oil, increasing your Critical Strike and Haste by 232 for 120 min.\r\n\r\n\"The oil radiates a subtle, but noticeable magical aura.\""
				},
				new Item
			{
				Id = 8,
				Name = "Crystallized Augment Rune",
				Icon = "inv_10_enchanting_crystal_color5.jpg",
				ItemLevel = 80,
				Quality = "Rare",
				RequiredLevel = 80,
				Url = "https://www.wowhead.com/item=224572/crystallized-augment-rune",
				Description = "Use: Increases Primary Stat by 733 for 1 hour. Augment Rune.\r\n\r\n\"Can be bought and sold on the auction house.\""
			},

			new Item
			{
				Id = 9,
				Name = "Feast of the Divine Day",
				Icon = "inv_11_cooking_profession_feast_table01.jpg",
				ItemLevel = 80,
				Quality = "Epic",
				RequiredLevel = 68,
				Url = "https://www.wowhead.com/item=222732/feast-of-the-divine-day",
				Description = "Use: Place a Mereldar Feast for all players to enjoy!\r\n\r\nRestores 9000000 health and 3000000 mana over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 446 primary stat for 1 hour."
			},

			new Item
			{
				Id = 10,
				Name = "Elusive Blasphemite",
				Icon = "inv_misc_gem_x4_metagem_cut.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Epic",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213746/elusive-blasphemite",
				Description = "Unique-Equipped: Algari Diamond (1)\r\n+181 Primary Stat and +2% Movement Speed per unique Algari gem color"
			},

			new Item
			{
				Id = 11,
				Name = "Quick Onyx",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color2_3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},

			new Item
			{
				Id = 12,
				Name = "Flask of Tempered Mastery",
				Icon = "inv_potion_purlple.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Common",
				RequiredLevel = 71,
				Url = "https://www.wowhead.com/item=212280/flask-of-tempered-mastery",
				Description = "Use: Drink to increase your Mastery by 2825.\r\n\r\nLasts 1 hour and through death. Consuming an identical flask will add another 1 hour."
			},

			new Item
			{
				Id = 13,
				Name = "Slumbering Soul Serum",
				Icon = "inv_flask_green.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Common",
				RequiredLevel = 71,
				Url = "https://www.wowhead.com/item=212247/slumbering-soul-serum",
				Description = "Use: Elevate your focus to restore 375000 mana over 10 sec, but you are defenseless until your focus is broken."
			},

			new Item
			{
				Id = 14,
				Name = "Jester's Board",
				Icon = "inv_misc_food_meat_cooked_06.jpg",
				ItemLevel = 80,
				Quality = "Rare",
				RequiredLevel = 68,
				Url = "https://www.wowhead.com/item=222730/jesters-board",
				Description = "Use: Restores 5400000 health over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 470 of your highest secondary stat for 1 hour."
			},

			new Item
			{
				Id = 15,
				Name = "Deadly Emerald",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color1_2.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213479/deadly-emerald",
				Description = "+147 Haste and +49 Critical Strike"
			},

			new Item
			{
				Id = 16,
				Name = "Tempered Potion",
				Icon = "trade_alchemy_potiona4.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Common",
				RequiredLevel = 71,
				Url = "https://www.wowhead.com/item=212265/tempered-potion",
				Description = "Use: Gain the effects of all inactive Tempered Flasks, increasing their associated secondary stats by 2617 for 30 sec."
			},
			new Item
			{
				Id = 17,
				Name = "Feast of the Midnight Masquerade",
				Icon = "inv_11_cooking_profession_feast_table02.jpg",
				ItemLevel = 80,
				Quality = "Epic",
				RequiredLevel = 68,
				Url = "https://www.wowhead.com/item=222733/feast-of-the-midnight-masquerade",
				Description = "Use: Place a Mereldar Feast for all players to enjoy!\r\n\r\nRestores 9000000 health and 3000000 mana over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 446 primary stat for 1 hour."
			},

			new Item
			{
				Id = 18,
				Name = "Outsider's Provisions",
				Icon = "inv_cooking_10_draconicdelicacies.jpg",
				ItemLevel = 80,
				Quality = "Rare",
				RequiredLevel = 68,
				Url = "https://www.wowhead.com/item=222731/outsiders-provisions",
				Description = "Use: Restores 5400000 health over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 470 of your highest secondary stat for 1 hour."
			},

			new Item
			{
				Id = 19,
				Name = "Empress' Farewell",
				Icon = "inv_misc_food_meat_cooked_02_color02.jpg",
				ItemLevel = 80,
				Quality = "Rare",
				RequiredLevel = 68,
				Url = "https://www.wowhead.com/item=222729/empress-farewell",
				Description = "Use: Restores 5400000 health over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 470 of your highest secondary stat for 1 hour."
			},

			new Item
			{
				Id = 20,
				Name = "Culminating Blasphemite",
				Icon = "item_cutmetagemb.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Epic",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213743/culminating-blasphemite",
				Description = "Unique-Equipped: Algari Diamond (1)\r\n+181 Primary Stat and +0.15% Critical Effect per unique Algari gem color"
			},

			new Item
			{
				Id = 21,
				Name = "Beledar's Bounty",
				Icon = "inv_cooking_100_roastduck_color02.jpg",
				ItemLevel = 80,
				Quality = "Rare",
				RequiredLevel = 80,
				Url = "https://www.wowhead.com/item=222728/beledars-bounty",
				Description = "Use: Restores 5400000 health over 20 sec. Must remain seated while eating.\r\n\r\nWell Fed: If you spend at least 10 seconds eating you will become Well Fed and gain 470 of your highest secondary stat for 1 hour."
			},

			new Item
			{
				Id = 22,
				Name = "Masterful Emerald",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color1_3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213482/masterful-emerald",
				Description = "+147 Haste and +49 Mastery"
			},

			new Item
			{
				Id = 23,
				Name = "Masterful Ruby",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color4_1.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213458/masterful-ruby",
				Description = "+147 Critical Strike and +49 Mastery"
			},

			new Item
			{
				Id = 24,
				Name = "Quick Ruby",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color4_3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213455/quick-ruby",
				Description = "+147 Critical Strike and +49 Haste"
			},

			new Item
			{
				Id = 25,
				Name = "Quick Sapphire",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color5_3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213470/quick-sapphire",
				Description = "+147 Versatility and +49 Haste"
			},

			new Item
			{
				Id = 26,
				Name = "Quick Onyx",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color2_3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},

			new Item
			{
				Id = 27,
				Name = "Masterful Sapphire",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color5_1.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213473/masterful-sapphire",
				Description = "+147 Versatility and +49 Mastery"
			},
				new Item
			{
				Id = 28,
				Name = "Deadly Sapphire",
				Icon = "inv_jewelcrafting_cut-standart-gem-hybrid_color5_2.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213477/deadly-sapphire",
				Description = "+147 Versatility and +49 Critical Strike"
			},

			new Item
			{
				Id = 29,
				Name = "Daybreak Spellthread",
				Icon = "inv_10_tailoring_craftingoptionalreagent_enhancedspellthread_color3.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Epic",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=222896/daybreak-spellthread",
				Description = "Use: Apply Daybreak Spellthread to your leggings, permanently increasing its Intellect by 930 and increasing the wearer's maximum mana by 5%."
			},
			new Item
			{
				Id= 30,
				Name = "Enchant Weapon - Authority of the Depths",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel =  610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223784/enchant-weapon-authority-of-the-depths",
				Description = "Use: Nerubian Novelties\r\n\r\nPermanently enchants a weapon with the Authority of the Depths. Damaging foes may invoke it, applying Suffocating Darkness which periodically inflicts 35003 Shadow damage. The darkness may deepen up to 3 times. Cannot be applied to items lower than level 350.\r\nRequires Level 70"
			},

			new Item
			{
				Id = 31,
				Name = "Daybreak Spellthread",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=222896/daybreak-spellthread",
				Description = "Use: Apply Daybreak Spellthread to your leggings, permanently increasing its Intellect by 930 and increasing the wearer's maximum mana by 5%."
			},
			new Item
			{
				Id = 32,
				Name = "Daybreak Spellthread",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=222896/daybreak-spellthread",
				Description = "Use: Apply Daybreak Spellthread to your leggings, permanently increasing its Intellect by 930 and increasing the wearer's maximum mana by 5%."
},
			new Item
			{
				Id = 33,
				Name = "Daybreak Spellthread",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=222896/daybreak-spellthread",
				Description = "Use: Apply Daybreak Spellthread to your leggings, permanently increasing its Intellect by 930 and increasing the wearer's maximum mana by 5%."
				},

			new Item
			{
				Id = 34,
				Name = "Daybreak Spellthread",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Epic",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=222896/daybreak-spellthread",
				Description = "Use: Apply Daybreak Spellthread to your leggings, permanently increasing its Intellect by 930 and increasing the wearer's maximum mana by 5%."
},
			new Item
			{
				Id = 35,
				Name = "Weavercloth Spellthread",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 36,
				Name = "Enchant Boots - Defender's March",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 37,
				Name = "Enchant Ring - Glimmering Haste",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 38,
				Name = "Enchant Ring - Radiant Haste",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
															
			new Item
			{
				Id = 39,
				Name = "Enchant Chest - Crystalline Radiance",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 40,
				Name = "Enchant Cloak - Chant of Winged Grace",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223731/enchant-cloak-chant-of-winged-grace",
				Description = "\r\nUse: Nerubian Novelties - Tertiary Trivialities\r\n\r\nPermanently enchants a cloak with ancient magic that chants with power, granting 545 Avoidance and reducing fall damage by 0%. Cannot be applied to items lower than level 350."
			},
			new Item
			{
				Id = 41,
				Name = "Enchant Cloak - Whisper of Silken Avoidance",
				Icon = "inv_misc_enchantedscroll.jpg",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223728/enchant-cloak-whisper-of-silken-avoidance",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 42,
				Name = "Enchant Bracer - Whisper of Armored Leech",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 43,
				Name = "Enchant Bracer - Chant of Armored Leech",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Cloak - Whisper of Silken Leech",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Cloak - Chant of Leeching Fangs",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Weapon - Stonebound Artistry",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Weapon - Authority of Fiery Resolve",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=213494/quick-onyx",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Bracer - Chant of Armored Avoidance",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223713/enchant-bracer-chant-of-armored-avoidance",
				Description = "+147 Mastery and +49 Haste"
			},
			new Item
			{
				Id = 39,
				Name = "Enchant Bracer - Chant of Armored Speed",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223725/enchant-bracer-chant-of-armored-speed",
				Description = "+147 Mastery and +49 Haste"
			},

			new Item
			{
				Id = 39,
				Name = "Enchant Bracer - Chant of Armored Avoidance",
				Icon = "inv_misc_enchantedscroll.jpg",
				SecondIcon = "professions-chaticon-quality-tier3.png",
				ItemLevel = 610,
				Quality = "Rare",
				RequiredLevel = 70,
				Url = "https://www.wowhead.com/item=223713/enchant-bracer-chant-of-armored-avoidance",
				Description = "+147 Mastery and +49 Haste"
			},
		};
	}
}

