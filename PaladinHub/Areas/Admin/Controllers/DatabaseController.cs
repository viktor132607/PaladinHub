using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Areas.Admin.ViewModels;
using PaladinHub.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaladinHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DatabaseController : Controller
	{
		private readonly AppDbContext _db;

		public DatabaseController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string? entity = "Spells", string? search = null, int page = 1, int pageSize = 20)
		{
			if (!Enum.TryParse(entity, true, out AdminEntity which))
				which = AdminEntity.Spells;

			var vm = new AdminDatabaseIndexViewModel
			{
				Entity = which,
				Search = search ?? "",
				Page = page,
				PageSize = pageSize
			};

			if (which == AdminEntity.Spells)
			{
				var q = _db.Spells.AsNoTracking().AsQueryable();

				if (!string.IsNullOrWhiteSpace(vm.Search))
					q = q.Where(s => s.Name.Contains(vm.Search) || (s.Description ?? "").Contains(vm.Search));

				vm.Total = await q.CountAsync();
				vm.Spells = await q.OrderBy(s => s.Name)
								   .Skip((vm.Page - 1) * vm.PageSize)
								   .Take(vm.PageSize)
								   .ToListAsync();
			}
			else
			{
				var q = _db.Items.AsNoTracking().AsQueryable();

				if (!string.IsNullOrWhiteSpace(vm.Search))
					q = q.Where(i => i.Name.Contains(vm.Search) || (i.Description ?? "").Contains(vm.Search));

				vm.Total = await q.CountAsync();
				vm.Items = await q.OrderBy(i => i.Name)
								  .Skip((vm.Page - 1) * vm.PageSize)
								  .Take(vm.PageSize)
								  .ToListAsync();
			}

			return View(vm);
		}
	}
}
