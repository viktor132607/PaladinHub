using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PaladinProject.Models;
using System;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

public class ItemsService
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
				Irl = "https://www.wowhead.com/spell=469411/a-just-reward",
				Description = "Use: Apply Greater Rune of the Void Ritual to a helm.Gain Void Ritual, giving your spells and abilities a chance to increase all secondary stats by 82 every sec for 20 sec.\r\n\r\nCannot be applied to items lower than level 350. This effect is fleeting and will only work during The War Within Season 2."},
			
			new Item { Id =1, Name ="Lesser Rune of the Void Ritual", Icon = "inv_inscription_majorglyph20.jpg", ItemLevel = 610, Quality = "Epic", RequiredLevel = 70 , Irl = "https://www.wowhead.com/spell=469411/a-just-reward", Description = "Use: Apply Greater Rune of the Void Ritual to a helm.\r\n\r\nGain Void Ritual, giving your spells and abilities a chance to increase all secondary stats by 82 every sec for 20 sec.\r\n\r\nCannot be applied to items lower than level 350. This effect is fleeting and will only work during The War Within Season 2."},

		};
	}
}

