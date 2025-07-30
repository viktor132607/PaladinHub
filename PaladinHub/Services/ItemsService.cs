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

			new Item {
	Id = 4,
	Name = "Aureate Sentry’s Pledge",
	Icon = "inv_plate_raidpaladingoblin_d_01_helm.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=229244/aureate-sentrys-pledge?bonus=7981:12042:5878:11996",
	Description = "Dropped by One-Armed Bandit in Liberation of Undermine raid."
},

new Item {
	Id = 5,
	Name = "Strapped Rescue-Keg",
	Icon = "StrappedRescue-keg.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=221060/strapped-rescue-keg?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Cinderbrew Meadery dungeon."
},

new Item {
	Id = 6,
	Name = "Aureate Sentry’s Roaring Will",
	Icon = "inv_plate_raidpaladingoblin_d_01_shoulder.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=229242/aureate-sentrys-roaring-will?bonus=7981:12042:5878:11996",
	Description = "Dropped by Rik Reverb in Liberation of Undermine raid."
},

new Item {
	Id = 7,
	Name = "Test Pilot’s Go-Pack",
	Icon = "Test Pilot's Go-Pack.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228844/test-pilots-go-pack?bonus=7981:12042:5878:11996",
	Description = "Dropped by Sprocketmonger in Liberation of Undermine raid."
},

new Item {
	Id = 8,
	Name = "Aureate Sentry’s Encasement",
	Icon = "inv_plate_raidpaladingoblin_d_01_chest.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=229247/aureate-sentrys-encasement?bonus=7981:12042:5878:11996",
	Description = "Dropped by Sprocketmonger in Liberation of Undermine raid."
},

new Item {
	Id = 9,
	Name = "Revved-Up Vambraces",
	Icon = "Revved-Up Vambraces.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228868/revved-up-vambraces?bonus=7981:12042:5878:11996",
	Description = "Dropped by Vexie in Liberation of Undermine raid."
},

new Item {
	Id = 10,
	Name = "Jumpstarter’s Scaffold-Scrapers",
	Icon = "inv_plate_outdoorundermine_c_01_glove.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=234504/jumpstarters-scaffold-scrapers?bonus=657:12042:5878:7981:11996",
	Description = "Dropped in Operation: Floodgate dungeon."
},

new Item {
	Id = 11,
	Name = "Venture Contractor’s Floodlight",
	Icon = "Venture Contractor's Floodlight.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=234505/venture-contractors-floodlight?bonus=657:12042:5878:7981:11996",
	Description = "Dropped in Operation: Floodgate dungeon."
},

new Item {
	Id = 12,
	Name = "Aureate Sentry’s Legguards",
	Icon = "inv_plate_raidpaladingoblin_d_01_pant.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=229243/aureate-sentrys-legguards?bonus=7981:12042:5878:11996",
	Description = "Dropped by Stix Bunkjunker in Liberation of Undermine raid."
},

new Item {
	Id = 13,
	Name = "Rik’s Walkin’ Boots",
	Icon = "Rik's Walkin' Boots.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228874/riks-walkin-boots?bonus=7981:12042:5878:11996",
	Description = "Dropped by Rik Reverb in Liberation of Undermine raid."
},

new Item {
	Id = 14,
	Name = "The Jastor Diamond",
	Icon = "TheJastorDiamond.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=231265/the-jastor-diamond?bonus=7981:12042:5878:11996",
	Description = "Dropped by Chrome King Gallywix in Liberation of Undermine raid."
},

new Item {
	Id = 15,
	Name = "Miniature Roulette Wheel",
	Icon = "MiniatureRouletteWheel.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228843/miniature-roulette-wheel?bonus=7981:12042:5878:11996",
	Description = "Dropped by One-Armed Bandit in Liberation of Undermine raid."
},

new Item {
	Id = 16,
	Name = "Eye of Kezan",
	Icon = "EyeOfKezan.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230198/eye-of-kezan?bonus=7981:12042:5878:11996",
	Description = "Dropped by Chrome King Gallywix in Liberation of Undermine raid."
},

new Item {
	Id = 17,
	Name = "Mister Pick-Me-Up",
	Icon = "Misterpick-me-up.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230186/mister-pick-me-up?bonus=7981:12042:5878:11996",
	Description = "Dropped by Sprocketmonger in Liberation of Undermine raid."
},

new Item {
	Id = 18,
	Name = "Big Earner’s Bludgeon",
	Icon = "BigEarner'sBludgeon.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228901/big-earners-bludgeon?bonus=7981:12042:5878:11996",
	Description = "Dropped by Mug’zee, Heads of Security in Liberation of Undermine raid."
},

new Item {
	Id = 19,
	Name = "Titan of Industry",
	Icon = "TitanOfIndustry.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228889/titan-of-industry?bonus=7981:12042:5878:11996",
	Description = "Dropped by Chrome King Gallywix in Liberation of Undermine raid."
},

	new Item {
	Id = 20,
	Name = "Gobfather's Gifted Bling",
	Icon = "StrappedRescue-keg.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228842/gobfathers-gifted-bling?bonus=7981:12042:5878:11996",
	Description = "Dropped by Mug’zee, Heads of Security in Liberation of Undermine raid."
},

new Item {
	Id = 21,
	Name = "Dumpmech Compactors",
	Icon = "inv_glove_plate_raidwarriorgoblin_d_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228849/dumpmech-compactors?bonus=7981:12042:5878:11996",
	Description = "Dropped by Stix Bunkjunker in Liberation of Undermine raid."
},

new Item {
	Id = 22,
	Name = "Coin-Operated Girdle",
	Icon = "inv_plate_raidpaladingoblin_d_01_belt.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228886/coin-operated-girdle?bonus=7981:12042:5878:11996",
	Description = "Dropped by One-Armed Bandit in Liberation of Undermine raid."
},

new Item {
	Id = 23,
	Name = "Cloak of Questionable Intent",
	Icon = "inv_bracer_plate_earthendungeon_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=159287/cloak-of-questionable-intent?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from The MOTHERLODE!! dungeon."
},

new Item {
	Id = 24,
	Name = "Slashproof Business Plate",
	Icon = "inv_chest_plate_earthendungeon_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=221069/slashproof-business-plate?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Cinderbrew Meadery dungeon."
},

new Item {
	Id = 25,
	Name = "Fuzzy Cindercuffs",
	Icon = "inv_bracer_plate_earthendungeon_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=221064/fuzzy-cindercuffs?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Cinderbrew Meadery dungeon."
},

new Item {
	Id = 26,
	Name = "Sabatons of Rampaging Elements",
	Icon = "inv_boot_plate_zandalardungeon_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=159679/sabatons-of-rampaging-elements?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from The MOTHERLODE!! dungeon."
},

new Item {
	Id = 27,
	Name = "Wick's Golden Loop",
	Icon = "inv_11_0_earthen_earthenring_color2.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=221099/wicks-golden-loop?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Darkflame Cleft dungeon."
},

new Item {
	Id = 28,
	Name = "Bloodoath Signet",
	Icon = "inv_ring_maldraxxus_01_red.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=178871/bloodoath-signet?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Theater of Pain dungeon."
},

new Item {
	Id = 29,
	Name = "Signet of the Priory",
	Icon = "SignetOfThePriory.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219308/signet-of-the-priory?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Priory of the Sacred Flame dungeon."
},

new Item {
	Id = 30,
	Name = "Carved Blazikon Wax",
	Icon = "CarvedBlazikonWax.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219305/carved-blazikon-wax?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Darkflame Cleft dungeon."
},

new Item {
	Id = 31,
	Name = "Electrifying Cognitive Amplifier",
	Icon = "inv_mace_1h_enprofession_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=168955/electrifying-cognitive-amplifier?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from Operation: Mechagon."
},

new Item {
	Id = 32,
	Name = "Galebreaker Bulwark",
	Icon = "inv_shield_1h_earthendungeon_c_02.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=221045/galebreaker-bulwark?bonus=657:12042:5878:7981:11996",
	Description = "Dropped from The Rookery dungeon."
},

new Item {
	Id = 33,
	Name = "Eye of Kezan",
	Icon = "EyeOfKezan.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230198/eye-of-kezan",
	Description = "Top-tier trinket for Holy Paladins in raid settings with strong passive effects."
},

new Item {
	Id = 34,
	Name = "Mister Pick-Me-Up",
	Icon = "MisterPick-Me-Up.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230186/mister-pick-me-up",
	Description = "Excellent trinket with on-use burst healing potential, great for all content types."
},
new Item {
	Id = 35,
	Name = "Mug's Moxie Jug",
	Icon = "Mug'sMoxieJug.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230192/mugs-moxie-jug",
	Description = "Strong burst healing trinket when paired with cooldowns like Avenging Wrath."
},

new Item {
	Id = 36,
	Name = "Reverb Radio",
	Icon = "Reverbradio.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230194/reverb-radio",
	Description = "Situational trinket with potential for high output in AoE encounters."
},

new Item {
	Id = 37,
	Name = "Carved Blazikon Wax",
	Icon = "CarvedBlazikonWax.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219305/carved-blazikon-wax",
	Description = "Solid passive trinket providing consistent stats over time."
},

new Item {
	Id = 38,
	Name = "Signet of the Priory",
	Icon = "SignetOfThePriory.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219308/signet-of-the-priory",
	Description = "Alternative trinket useful for builds stacking Mastery."
},

new Item {
	Id = 39,
	Name = "Algari Alchemist Stone",
	Icon = "AlgarialchemistStone.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=207160/algari-alchemist-stone",
	Description = "Crafted alchemy trinket providing a small stat proc and versatility."
},

new Item {
	Id = 40,
	Name = "Soulletting Ruby",
	Icon = "SoullettingRuby.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=178809/soulletting-ruby",
	Description = "Old Shadowlands trinket still useful with strong on-use Crit boost."
},

new Item {
	Id = 41,
	Name = "Unstable Power Suit Core",
	Icon = "Unstable Power Suit Core.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=169304/unstable-power-suit-core",
	Description = "Moderate trinket option with situational value depending on uptime."
},

new Item {
	Id = 42,
	Name = "Funhouse Lens",
	Icon = "Funhouse Lens.png",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230199/funhouse-lens",
	Description = "Offers some healing utility and randomness, not always reliable."
},

new Item {
	Id = 43,
	Name = "House of Cards",
	Icon = "HouseOfCards.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230193/house-of-cards",
	Description = "Mediocre stat trinket, can be used in the absence of better options."
},

new Item {
	Id = 44,
	Name = "Entropic Skardyn Core",
	Icon = "Entropic Skardyn Core.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219306/entropic-skardyn-core",
	Description = "Very situational trinket; decent in AoE burst windows but lacks consistency."
},
new Item {
	Id = 45,
	Name = "Sigil of Algari Concordance",
	Icon = "Sigil of Algari Concordance.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230204/sigil-of-algari-concordance",
	Description = "Low-value trinket with minor stat benefits; better options exist."
},

new Item {
	Id = 46,
	Name = "Darkfuse Medichopper",
	Icon = "Darkfuse Medichopper.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230197/darkfuse-medichopper",
	Description = "Mobile healing support trinket with limited effectiveness."
},

new Item {
	Id = 47,
	Name = "Burin of the Candle King",
	Icon = "Burin of the Candle King.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230196/burin-of-the-candle-king",
	Description = "Thematic trinket with minor passive benefits; not ideal for performance."
},

new Item {
	Id = 48,
	Name = "Synergistic Brewtializer",
	Icon = "Synergistic Brewterializer.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230203/synergistic-brewtializer",
	Description = "Provides group-wide effects but low personal value."
},

new Item {
	Id = 49,
	Name = "Vial of Spectral Essence",
	Icon = "Vial of Spectral Essence.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230195/vial-of-spectral-essence",
	Description = "Outdated trinket with weaker throughput effects."
},

new Item {
	Id = 50,
	Name = "Remnant of Darkness",
	Icon = "Remnant of Darkness.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230200/remnant-of-darkness",
	Description = "Shadow-based effect with low synergy for healing builds."
},

new Item {
	Id = 51,
	Name = "Ingenious Mana Battery",
	Icon = "Ingenious Mana Battery.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230201/ingenious-mana-battery",
	Description = "Restores mana, but weak compared to modern trinkets."
},

new Item {
	Id = 52,
	Name = "Gallagio Bottle Service",
	Icon = "GallagioBottleService.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=230202/gallagio-bottle-service",
	Description = "Flavorful but not powerful — mostly for fun or RP."
},
new Item {
	Id = 53,
	Name = "Charged Hexsword",
	Icon = "inv_sword_1h_arathor_c_01.jpg",
	ItemLevel = 676,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=222444/charged-hexsword?bonus=12040:1515",
	Description = "Crafted 1H weapon. Recommended for Holy Paladin early gearing. Often paired with Darkmoon Sigil: Ascension."
},

new Item {
	Id = 54,
	Name = "Darkmoon Sigil: Ascension",
	Icon = "DarkmoonSigilAscension.jpg",
	ItemLevel = 676,
	Quality = "Rare",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=226024/darkmoon-sigil-ascension",
	Description = "Best weapon embellishment for Holy Paladin. Scales over time during long encounters. Effect doubled by Writhing Armor Banding."
},

new Item {
	Id = 55,
	Name = "Writhing Armor Banding",
	Icon = "inv_10_skinning_craftedoptionalreagent_studdedleatherswatch_color2.jpg",
	ItemLevel = 676,
	Quality = "Rare",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=219506/writhing-armor-banding",
	Description = "Optional crafting reagent that doubles effect of Nerubian embellishments like Darkmoon Sigil: Ascension."
},

new Item {
	Id = 56,
	Name = "Dawnthread Lining",
	Icon = "inv_10_tailoring_tailoringconsumable_color2.jpg",
	ItemLevel = 676,
	Quality = "Rare",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=222870/dawnthread-lining",
	Description = "Minor but consistent healing embellishment, recommended for armor crafts that won't be replaced."
},

new Item {
	Id = 57,
	Name = "Algari Missive of the Feverflare",
	Icon = "inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg",
	ItemLevel = 676,
	Quality = "Uncommon",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=222584/algari-missive-of-the-feverflare",
	Description = "Adds Haste + Mastery to crafted gear. Ideal stat combo for raid-focused builds."
},

new Item {
	Id = 58,
	Name = "Algari Missive of the Fireflash",
	Icon = "inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg",
	ItemLevel = 676,
	Quality = "Uncommon",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=222587/algari-missive-of-the-fireflash",
	Description = "Adds Haste + Crit to crafted gear. Best for Mythic+ and bursty builds."
},
new Item {
	Id = 59,
	Name = "Cyrce's Circlet",
	Icon = "Cyrce's Circlet.jpg",
	ItemLevel = 660,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/guide/the-war-within/patch-11-0-7-cyrces-circlet-customizable-ring",
	Description = "Customizable ring introduced in patch 11.0.7. Can be upgraded and socketed with Runed Citrine gems."
},

new Item {
	Id = 60,
	Name = "Stormbringer's Runed Citrine",
	Icon = "stormbringer-icon.jpg",
	ItemLevel = 660,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228638/stormbringers-runed-citrine",
	Description = "Gem for Cyrce's Circlet. Adds bonus damage and healing after casting spells. Recommended for all builds."
},

new Item {
	Id = 61,
	Name = "Fathomdweller's Runed Citrine",
	Icon = "fathomdweller-icon.jpg",
	ItemLevel = 660,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228639/fathomdwellers-runed-citrine",
	Description = "Gem for Cyrce's Circlet. Boosts healing based on Mastery. Scales well for Herald of the Sun builds."
},

new Item {
	Id = 62,
	Name = "Windsinger's Runed Citrine",
	Icon = "windsinger-icon.jpg",
	ItemLevel = 660,
	Quality = "Epic",
	RequiredLevel = 70,
	Url = "https://www.wowhead.com/item=228640/windsingers-runed-citrine",
	Description = "Gem for Cyrce's Circlet. Improves movement speed and healing output briefly after casting."
},

		};
	}
}

