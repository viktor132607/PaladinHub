using System.Collections.Generic;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;

namespace PaladinHub.Services.TalentTrees
{
	public interface IClassTreeBuilder
	{
		/// <summary>Базов ключ за дървото, напр. "paladin".</summary>
		string BaseKey { get; }

		/// <summary>Билдва класовото дърво (не-hero, не-специализация).</summary>
		TalentTreeViewModel BuildTree(List<Spell> spells);
	}
}
