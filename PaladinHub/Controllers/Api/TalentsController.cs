using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Models;
using PaladinHub.Services.TalentTrees;

namespace PaladinHub.Controllers
{
	public class TalentsController : Controller
	{
		private readonly AppDbContext _db;
		private readonly ITalentTreeService _talentTrees;

		public TalentsController(AppDbContext db, ITalentTreeService talentTrees)
		{
			_db = db;
			_talentTrees = talentTrees;
		}

		[HttpGet]
		[Route("{section:palsec}/Talents")]
		public async Task<IActionResult> Talents(string section)
		{
			var sec = section?.Trim().ToLowerInvariant();
			if (sec is null) return NotFound();

			var spells = await _db.Spells.AsNoTracking().ToListAsync();
			var items = await _db.Items.AsNoTracking().ToListAsync();

			var model = new CombinedViewModel
			{
				PageTitle = $"{Cap(sec)} Paladin – Talents",
				Spells = spells,
				Items = items
			};

			model.TalentTrees = await _talentTrees.GetTalentTrees(sec, spells);

			ViewData["ShowTalentTrees"] = true;
			ViewData["TalentTreeKeys"] = KeysFor(sec); // подредба/филтър за _SectionTalentTrees

			var viewPath = $"~/Views/{Cap(sec)}/Talents.cshtml";
			return View(viewPath, model);
		}

		private static string Cap(string s)
			=> string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s[1..];

		private static IEnumerable<string> KeysFor(string sec) => sec switch
		{
			"holy" => new[] { "paladin", "holy-herald", "holy" },             // ляво | среда | дясно
			"protection" => new[] { "paladin", "protection-lightsmith", "protection" },
			"retribution" => new[] { "paladin", "retribution-templar", "retribution" },
			_ => Array.Empty<string>()
		};
	}
}
