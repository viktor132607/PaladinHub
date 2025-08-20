using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace PaladinHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ItemsController : Controller
	{
		private readonly AppDbContext _db;
		public ItemsController(AppDbContext db) => _db = db;

		[HttpGet]
		public async Task<IActionResult> Create() => View(new Item());

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Item item)
		{
			if (!ModelState.IsValid) return View(item);

			_db.Items.Add(item);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Items" });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var item = await _db.Items.FindAsync(id);
			return item == null ? NotFound() : View(item);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Item item)
		{
			if (id != item.Id) return BadRequest();
			if (!ModelState.IsValid) return View(item);

			_db.Entry(item).State = EntityState.Modified;
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Items" });
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var item = await _db.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
			return item == null ? NotFound() : View(item);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var item = await _db.Items.FindAsync(id);
			return item == null ? NotFound() : View(item);
		}

		[HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var item = await _db.Items.FindAsync(id);
			if (item == null) return NotFound();

			_db.Items.Remove(item);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Database", new { entity = "Items" });
		}
	}
}
