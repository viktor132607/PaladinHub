using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;

namespace PaladinHub.Services.TalentTrees
{
	public class TalentTreeService : ITalentTreeService
	{
		private readonly IEnumerable<ISpecializationTreeBuilder> _specBuilders;
		private readonly IHeroTalentTreesService _heroTrees;
		private readonly IClassTreeBuilder _classTree;
		private readonly ITalentTreeAdminService _adminStates;

		public TalentTreeService(
			IEnumerable<ISpecializationTreeBuilder> specBuilders,
			IHeroTalentTreesService heroTrees,
			IClassTreeBuilder classTree,
			ITalentTreeAdminService adminStates)
		{
			_specBuilders = specBuilders ?? Array.Empty<ISpecializationTreeBuilder>();
			_heroTrees = heroTrees ?? throw new ArgumentNullException(nameof(heroTrees));
			_classTree = classTree ?? throw new ArgumentNullException(nameof(classTree));
			_adminStates = adminStates ?? throw new ArgumentNullException(nameof(adminStates));
		}

		/// <summary>
		/// Връща речник с всички дървета за секция (paladin class + 1 spec + 1–2 hero),
		/// като налага админ състоянията за активност на нодовете от БД.
		/// </summary>
		public async Task<Dictionary<string, TalentTreeViewModel>> GetTalentTrees(string section, List<Spell> spells)
		{
			var dict = new Dictionary<string, TalentTreeViewModel>(StringComparer.OrdinalIgnoreCase);
			spells ??= new List<Spell>();

			string sec = Normalize(section); // "holy" | "protection" | "retribution"

			// ===== Paladin class tree (ляво)
			var classVm = _classTree.BuildTree(spells);
			if (classVm != null && !string.IsNullOrWhiteSpace(classVm.Key))
				dict[classVm.Key] = classVm; // очаквано: "paladin"

			// ===== Spec tree (дясно)
			var specBuilder = _specBuilders.FirstOrDefault(b =>
				string.Equals(Normalize(b.BaseKey), sec, StringComparison.OrdinalIgnoreCase));

			if (specBuilder != null)
			{
				var specVm = specBuilder.BuildTree(spells); // "holy" | "protection" | "retribution"
				if (specVm != null && !string.IsNullOrWhiteSpace(specVm.Key))
					dict[specVm.Key] = specVm;
			}

			// ===== Hero trees (средата) – обикновено 2
			var heroTrees = _heroTrees.GetHeroTrees(sec, spells);
			if (heroTrees != null)
			{
				foreach (var kv in heroTrees)
				{
					// напр. "holy-herald", "holy-lightsmith", "protection-templar", ...
					if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Value != null)
						dict[kv.Key] = kv.Value;
				}
			}

			// ===== Налагане на админ състоянията (Active) за всеки нод
			foreach (var tree in dict.Values)
				await ApplyAdminStatesAsync(tree);

			return dict;
		}

		/// <summary>
		/// Връща единично дърво по ключ и секция, с наложени админ състояния.
		/// </summary>
		public async Task<TalentTreeViewModel?> GetTalentTree(string key, string section, List<Spell> spells)
		{
			if (string.IsNullOrWhiteSpace(key))
				return null;

			var all = await GetTalentTrees(section, spells);
			return all.TryGetValue(key, out var vm) ? vm : null;
		}

		private static string Normalize(string s)
			=> (s ?? string.Empty).Trim().ToLowerInvariant();

		/// <summary>
		/// Зарежда от БД {NodeId -> IsActive} за даденото дърво и override-ва n.Active.
		/// </summary>
		private async Task ApplyAdminStatesAsync(TalentTreeViewModel tree)
		{
			if (tree == null || string.IsNullOrWhiteSpace(tree.Key) || tree.Nodes == null || tree.Nodes.Count == 0)
				return;

			var states = await _adminStates.GetStatesAsync(tree.Key);
			if (states == null || states.Count == 0) return;

			foreach (var n in tree.Nodes)
			{
				if (n == null || string.IsNullOrWhiteSpace(n.Id)) continue;
				if (states.TryGetValue(n.Id, out var isActive))
					n.Active = isActive;
			}
		}
	}
}
