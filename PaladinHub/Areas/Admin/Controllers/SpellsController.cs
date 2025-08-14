using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;

namespace PaladinHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SpellsController : Controller
	{
		private readonly AppDbContext _db;
		public SpellsController(AppDbContext db) => _db = db;

		[HttpGet]
		public async Task<IActionResult> Create() => View(new Spell());

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Spell spell)
		{
			if (!ModelState.IsValid) return View(spell);

			_db.Spells.Add(spell);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Spells" });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var spell = await _db.Spells.FindAsync(id);
			return spell == null ? NotFound() : View(spell);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Spell spell)
		{
			if (id != spell.Id) return BadRequest();
			if (!ModelState.IsValid) return View(spell);

			_db.Entry(spell).State = EntityState.Modified;
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Spells" });
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var spell = await _db.Spells.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
			return spell == null ? NotFound() : View(spell);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var spell = await _db.Spells.FindAsync(id);
			return spell == null ? NotFound() : View(spell);
		}

		[HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var spell = await _db.Spells.FindAsync(id);
			if (spell == null) return NotFound();

			_db.Spells.Remove(spell);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Spells" });
		}
	}
}
