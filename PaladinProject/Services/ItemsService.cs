using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PaladinProject.Models;
using System;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

public class ItemsService
{
	public List<Spell> GetAllItems()
	{
		return new List<Spell>
		{
			new Spell { Id =1, Name ="A Just Reward", Icon = "A Just Reward.jpg", Irl = "https://www.wowhead.com/spell=469411/a-just-reward", Description = "Protection,  Retribution After Cleanse Toxins successfully removes an effect from an ally, they are healed for (540% of Spell power). Holy After Cleanse successfully removes an effect from an ally, they are healed for (540% of Spell power)."},
			new Spell { Id =2, Name ="Adjudication", Icon = "Ardent Defender.jpg", Irl = "https://www.wowhead.com/spell=406157/adjudication", Description = "Critical Strike damage of your abilities increased by 5% and Hammer of Wrath also has a chance to cast Highlord's Judgment."},
			new Spell { Id =3, Name ="Aegis of Light", Icon = "Ashes to Ashes.jpg", Irl = "https://www.wowhead.com/spell=204150/aegis-of-light", Description = "Channels an Aegis of Light that protects you and all allies standing within 10 yards behind you for 6 sec, reducing all damage taken by 20%."},

		};
	}
}

