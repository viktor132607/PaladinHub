using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;
using PaladinProject.ViewModels;

namespace PaladinProject.Controllers
{
    [Route("Retribution")]
    public class RetributionController : Controller
    {
		private readonly ILogger<HolyController> _logger;
		private readonly SpellbookService _spellbookService;

		public RetributionController(ILogger<HolyController> logger)
		{
			_logger = logger;
			_spellbookService = new SpellbookService(); // ��� DI �� � ���������
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => View();

		[HttpGet("Talents")]
		public IActionResult Talents() => View();

		[HttpGet("Stats")]
		public IActionResult Stats()
		{
			// ��������� ������ ������� �� SpellbookService
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
