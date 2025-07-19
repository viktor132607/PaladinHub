using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaladinProject.Models;
using PaladinProject.Services;
using PaladinProject.ViewModels;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
	[Route("Holy")]
	public class HolyController : BaseController
	{
		private readonly ILogger<HolyController> _logger;

		public HolyController(
			ILogger<HolyController> logger,
			ISpellbookService spellbookService,
			IItemsService itemsService
		) : base(spellbookService, itemsService)
		{
			_logger = logger;
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => View();

		[HttpGet("Talents")]
		public IActionResult Talents() => View(SpellbookService.GetAllSpells());

		[HttpGet("Stats")]
		public IActionResult Stats() => View(SpellbookService.GetAllSpells());

		[HttpGet("Consumables")]
		public IActionResult Consumables() => View(ItemsService.GetAllItems());

		[HttpGet("Gear")]
		public IActionResult Gear() => View();

		[HttpGet("Rotation")]
		public IActionResult Rotation() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
