using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;
using PaladinProject.ViewModels;

namespace PaladinProject.Controllers
{
    [Route("Retribution")]
    public class RetributionController : BaseController
    {
		private readonly ILogger<HolyController> _logger;
		private readonly SpellbookService _spellbookService;

		public RetributionController(ILogger<HolyController> logger)
		{
			_logger = logger;
			_spellbookService = new SpellbookService(); // àêî DI íå å àêòèâèðàí
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => View();

		[HttpGet("talents")]
		public IActionResult Talents()
		{
			// Çàðåæäàìå âñè÷êè ñïåëîâå îò SpellbookService
			var allSpells = _spellbookService.GetAllSpells();
			return View(allSpells);
		}

		[HttpGet("Stats")]
		public IActionResult Stats()
		{
			// Çàðåæäàìå âñè÷êè ñïåëîâå îò SpellbookService
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
