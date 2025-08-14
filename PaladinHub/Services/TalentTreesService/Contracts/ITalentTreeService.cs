using System.Collections.Generic;
using System.Threading.Tasks;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;

namespace PaladinHub.Services.TalentTrees
{
	public interface ITalentTreeService
	{
		Task<Dictionary<string, TalentTreeViewModel>> GetTalentTrees(string section, List<Spell> spells);
		Task<TalentTreeViewModel?> GetTalentTree(string key, string section, List<Spell> spells);
	}
}
