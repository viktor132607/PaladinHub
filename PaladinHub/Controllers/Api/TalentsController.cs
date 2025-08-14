using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Models;
using PaladinHub.Services.TalentTrees;

namespace PaladinHub.Controllers
{
	// Вече не е MVC Controller, няма маршрути и IActionResult.
	// Регистрирай го в DI и го викай от PaladinController.
	public class TalentsController
	{
		private readonly AppDbContext _db;
		private readonly ITalentTreeService _talentTrees;

		public TalentsController(AppDbContext db, ITalentTreeService talentTrees)
		{
			_db = db;
			_talentTrees = talentTrees;
		}

		public async Task<(CombinedViewModel Model, IEnumerable<string> Keys, string ViewPath)> BuildPageAsync(string section)
		{
			if (string.IsNullOrWhiteSpace(section))
				throw new ArgumentException("section is required", nameof(section));

			var sec = section.Trim().ToLowerInvariant();

			var spells = await _db.Spells.AsNoTracking().ToListAsync();
			var items = await _db.Items.AsNoTracking().ToListAsync();

			var model = new CombinedViewModel
			{
				PageTitle = $"{Cap(sec)} Paladin – Talents",
				Spells = spells,
				Items = items
			};

			model.TalentTrees = await _talentTrees.GetTalentTrees(sec, spells);

			var keys = KeysFor(sec);
			var viewPath = $"~/Views/{Cap(sec)}/Talents.cshtml";

			return (model, keys, viewPath);
		}

		private static string Cap(string s)
			=> string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s[1..];

		private static IEnumerable<string> KeysFor(string sec) => sec switch
		{
			"holy" => new[] { "paladin", "holy-herald", "holy" },
			"protection" => new[] { "paladin", "protection-lightsmith", "protection" },
			"retribution" => new[] { "paladin", "retribution-templar", "retribution" },
			_ => Array.Empty<string>()
		};
	}
}
