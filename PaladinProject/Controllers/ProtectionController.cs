using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaladinProject.Data;
using PaladinProject.Helpers;
using PaladinProject.Models;
using PaladinProject.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace PaladinProject.Controllers
{
	[Route("Protection")]
	public class ProtectionController : Controller
	{
		private readonly ILogger<HolyController> _logger;
		private readonly SpellbookService _spellbookService;

		public ProtectionController(ILogger<HolyController> logger)
		{
			_logger = logger;
			_spellbookService = new SpellbookService(); // ако DI не е активиран
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => View();

		[HttpGet("Talents")]
		public IActionResult Talents()
		{
			// «ареждаме всички спелове от SpellbookService
			var allSpells = _spellbookService.GetAllSpells();
			return View(allSpells);
		}

		[HttpGet("Stats")]
		public IActionResult Stats()
		{
			// «ареждаме всички спелове от SpellbookService
			var allSpells = _spellbookService.GetAllSpells();
			return View(allSpells);
		}

		[HttpGet("Consumables")]
		public IActionResult Consumables() => View();

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
