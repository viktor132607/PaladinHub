using System.Collections.Generic;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;

namespace PaladinHub.Services.TalentTrees
{
	/// <summary>Строи Hero дърветата за дадената специализация.</summary>
	public interface IHeroTalentTreesService
	{
		Dictionary<string, TalentTreeViewModel> GetHeroTrees(string specialization, List<Spell> spells);
		TalentTreeViewModel? GetHeroTree(string specialization, string key, List<Spell> spells);
	}
}
