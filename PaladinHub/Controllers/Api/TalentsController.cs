using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Models;
using PaladinHub.Services.TalentTrees;
using PaladinHub.Models.Talents;
using System.Linq;

namespace PaladinHub.Controllers
{
	[Route("talents")]
	public class TalentsController : Controller
	{
		private readonly AppDbContext _db;
		private readonly ITalentTreeService _talentTrees;

		public TalentsController(AppDbContext db, ITalentTreeService talentTrees)
		{
			_db = db;
			_talentTrees = talentTrees;
		}

		private static string NormalizeSection(string s)
		{
			var x = (s ?? "").Trim().ToLowerInvariant();
			return x switch
			{
				"holy" or "holly" => "holy",
				"prot" or "protection" => "protection",
				"ret" or "retri" or "retribution" => "retribution",
				_ => x
			};
		}
		private static string TitleCase(string s)
			=> string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + (s.Length > 1 ? s[1..] : "");

		private static string[] BuildKeysForSection(
			IReadOnlyDictionary<string, TalentTreeViewModel> treeDict,
			string sec)
		{
			var keys = treeDict.Keys;
			var classKey = keys.FirstOrDefault(k => k.Equals("paladin", StringComparison.OrdinalIgnoreCase));

			string? PickHero(string alias)
				=> keys.FirstOrDefault(k =>
					   k.Contains(sec, StringComparison.OrdinalIgnoreCase)
					&& k.EndsWith("-" + alias, StringComparison.OrdinalIgnoreCase))
					?? keys.FirstOrDefault(k => k.EndsWith("-" + alias, StringComparison.OrdinalIgnoreCase));

			var heroKey = PickHero("herald") ?? PickHero("lightsmith") ?? PickHero("templar");

			var specKey = keys.FirstOrDefault(k => k.Equals(sec, StringComparison.OrdinalIgnoreCase))
						  ?? keys.FirstOrDefault(k => k.Contains(sec, StringComparison.OrdinalIgnoreCase));

			return new[] { classKey, heroKey, specKey }
				.Where(k => !string.IsNullOrWhiteSpace(k))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray()!;
		}

		private static string? ResolveKey(string raw, IEnumerable<string> candidates, string? fallbackSection = null)
		{
			if (string.IsNullOrWhiteSpace(raw)) return null;

			var key = raw.Trim().ToLowerInvariant();
			var list = candidates.ToList();

			var hit = list.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
			if (!string.IsNullOrEmpty(hit)) return hit;

			hit = list.FirstOrDefault(k => k.EndsWith(key, StringComparison.OrdinalIgnoreCase));
			if (!string.IsNullOrEmpty(hit)) return hit;

			var toks = key.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			if (toks.Length > 0)
			{
				hit = list.FirstOrDefault(k =>
				{
					var kt = k.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
					return toks.All(t => kt.Contains(t, StringComparer.OrdinalIgnoreCase));
				});
				if (!string.IsNullOrEmpty(hit)) return hit;
			}

			if (key is "herald" or "lightsmith" or "templar")
			{
				if (!string.IsNullOrWhiteSpace(fallbackSection))
				{
					hit = list.FirstOrDefault(k =>
						k.Contains(fallbackSection, StringComparison.OrdinalIgnoreCase) &&
						k.EndsWith("-" + key, StringComparison.OrdinalIgnoreCase));
					if (!string.IsNullOrEmpty(hit)) return hit;
				}
				hit = list.FirstOrDefault(k => k.EndsWith("-" + key, StringComparison.OrdinalIgnoreCase));
				if (!string.IsNullOrEmpty(hit)) return hit;
			}

			return null;
		}

		private async Task<CombinedViewModel> BuildModel(string section)
		{
			var sec = NormalizeSection(section);
			var spells = await _db.Spells.AsNoTracking().ToListAsync();
			var items = await _db.Items.AsNoTracking().ToListAsync();
			var trees = await _talentTrees.GetTalentTrees(sec, spells);

			return new CombinedViewModel
			{
				PageTitle = $"{TitleCase(sec)} Paladin – Talents",
				Spells = spells,
				Items = items,
				TalentTrees = trees
			};
		}

		// /talents/holy, /talents/protection, /talents/retribution   (пълна страница)
		[HttpGet("{section:regex(^holy|protection|retribution$)}")]
		public async Task<IActionResult> SectionPage(string section)
		{
			var model = await BuildModel(section);
			var sec = NormalizeSection(section);

			ViewData["Spells"] = model.Spells; // <-- нужно за _TalentTree.cshtml
			ViewData["keys"] = BuildKeysForSection(model.TalentTrees, sec);

			var viewPath = sec switch
			{
				"holy" => "~/Views/Holy/Talents.cshtml",
				"protection" => "~/Views/Protection/Talents.cshtml",
				_ => "~/Views/Retribution/Talents.cshtml"
			};
			return View(viewPath, model);
		}

		// GET /talents/all/{section}  → partial (paladin + hero + spec)
		[HttpGet("all/{section}")]
		public async Task<IActionResult> GetAll(string section)
		{
			if (string.IsNullOrWhiteSpace(section))
				return BadRequest("section is required.");

			var sec = NormalizeSection(section);
			var model = await BuildModel(sec);

			ViewData["Spells"] = model.Spells; // <-- важно
			ViewData["keys"] = BuildKeysForSection(model.TalentTrees, sec);

			return PartialView("~/Views/Shared/TalentTrees/_SectionTalentTrees.cshtml", model);
		}

		// GET /talents/tree/{key}?section=holy  → partial (конкретно дърво)
		[HttpGet("tree/{key}")]
		public async Task<IActionResult> GetTree(string key, [FromQuery] string? section = null)
		{
			if (string.IsNullOrWhiteSpace(key))
				return BadRequest("key is required.");

			var sec = NormalizeSection(section ?? key.Split('-').FirstOrDefault() ?? "");
			if (string.IsNullOrWhiteSpace(sec))
				return BadRequest("section could not be resolved from key; pass ?section=...");

			var model = await BuildModel(sec);

			var resolved = ResolveKey(key, model.TalentTrees.Keys, sec);
			if (resolved is null)
				return NotFound($"No tree found for key '{key}' in section '{sec}'.");

			ViewData["Spells"] = model.Spells; // <-- важно
			ViewData["keys"] = new[] { "paladin", resolved, sec };

			return PartialView("~/Views/Shared/TalentTrees/_SectionTalentTrees.cshtml", model);
		}
	}
}
