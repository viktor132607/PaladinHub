using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Areas.Admin.Models;
using PaladinHub.Data;
using PaladinHub.Data.Models;

namespace PaladinHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	[Route("Admin/PageBuilder")]
	public class PageBuilderController : Controller
	{
		private readonly AppDbContext _db;
		public PageBuilderController(AppDbContext db) => _db = db;

		private static string NormalizeSection(string? s)
		{
			var k = (s ?? "").Trim().ToLowerInvariant();
			return k switch
			{
				"holy" => "holy",
				"protection" or "prot" => "protection",
				"retribution" or "retri" or "ret" => "retribution",
				_ => "holy"
			};
		}

		private static string Cap(string s)
			=> string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s[1..];

		private static string Slugify(string? s)
		{
			var slug = (s ?? "").Trim().ToLowerInvariant();

			slug = new string(slug.Where(ch => char.IsLetterOrDigit(ch) || ch == '-').ToArray());
			slug = string.Join("-", slug.Split('-', StringSplitOptions.RemoveEmptyEntries));
			return string.IsNullOrWhiteSpace(slug) ? "page" : slug;
		}

		[HttpGet("Create")]
		public IActionResult Create([FromQuery] string? section)
		{
			var sec = NormalizeSection(section);
			return View(new CreatePageViewModel
			{
				Section = Cap(sec),
				Title = "",
				Slug = ""
			});
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreatePageViewModel vm, [FromForm] string jsonLayout)
		{
			var sec = NormalizeSection(vm.Section);

			var rawSlug = string.IsNullOrWhiteSpace(vm.Slug) ? vm.Title : vm.Slug;
			var slug = Slugify(rawSlug);

			if (!ModelState.IsValid) return View(vm);

			var exists = await _db.ContentPages.AnyAsync(p => p.Section == sec && p.Slug == slug);
			if (exists)
			{
				ModelState.AddModelError(nameof(vm.Slug), "Slug is already used in this section.");
				return View(vm);
			}

			var page = new ContentPage
			{
				Section = sec,
				Slug = slug,
				Title = string.IsNullOrWhiteSpace(vm.Title) ? slug : vm.Title.Trim(),
				IsPublished = true,
				JsonLayout = string.IsNullOrWhiteSpace(jsonLayout) ? "[]" : jsonLayout.Trim(),
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,

				RowVersion = Array.Empty<byte>()
			};

			_db.ContentPages.Add(page);
			await _db.SaveChangesAsync();

			return Redirect($"/{Cap(sec)}/{slug}");
		}

		[HttpGet("DeleteConfirm")]
		public async Task<IActionResult> DeleteConfirm([FromQuery] string section, [FromQuery] string slug)
		{
			var sec = NormalizeSection(section);
			var slg = Slugify(slug);

			var page = await _db.ContentPages.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Section == sec && p.Slug == slg);

			if (page == null) return NotFound();

			var vm = new DeletePageViewModel
			{
				Section = Cap(sec),
				Slug = page.Slug,
				Title = page.Title
			};

			return View("DeleteConfirm", vm);
		}

		[HttpGet("Delete")]
		public Task<IActionResult> Delete([FromQuery] string section, [FromQuery] string slug)
			=> DeleteConfirm(section, slug);

		[HttpPost("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(DeletePageViewModel vm)
		{
			var sec = NormalizeSection(vm.Section);
			var slg = Slugify(vm.Slug);

			var page = await _db.ContentPages.FirstOrDefaultAsync(p => p.Section == sec && p.Slug == slg);
			if (page != null)
			{
				_db.ContentPages.Remove(page);
				await _db.SaveChangesAsync();
			}

			return Redirect($"/{Cap(sec)}/Overview");
		}

		[HttpGet("Edit")]
		public async Task<IActionResult> Edit([FromQuery] string section, [FromQuery] string slug)
		{
			var sec = NormalizeSection(section);
			var slg = Slugify(slug);

			var page = await _db.ContentPages.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Section == sec && p.Slug == slg);

			if (page == null) return NotFound();

			var vm = new CreatePageViewModel
			{
				Section = Cap(sec),
				Title = page.Title,
				Slug = page.Slug
			};

			ViewBag.JsonLayout = page.JsonLayout;

			return View("Create", vm);
		}

		[HttpPost("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditPost(CreatePageViewModel vm, [FromForm] string jsonLayout)
		{
			var sec = NormalizeSection(vm.Section);
			var slg = Slugify(vm.Slug);

			var page = await _db.ContentPages.FirstOrDefaultAsync(p => p.Section == sec && p.Slug == slg);
			if (page == null) return NotFound();

			page.Title = string.IsNullOrWhiteSpace(vm.Title) ? page.Title : vm.Title.Trim();
			page.JsonLayout = string.IsNullOrWhiteSpace(jsonLayout) ? page.JsonLayout : jsonLayout.Trim();
			page.UpdatedAt = DateTime.UtcNow;

			await _db.SaveChangesAsync();

			return Redirect($"/{Cap(sec)}/{slg}");
		}
	}
}