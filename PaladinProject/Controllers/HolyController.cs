using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaladinProject.Models;
using PaladinProject.ViewModels;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
	[Route("Holy")]
	public class HolyController : BaseController
	{
		private readonly ILogger<HolyController> _logger;
		private readonly SpellbookService _spellbookService;

		public HolyController(ILogger<HolyController> logger)
		{
			_logger = logger;
			_spellbookService = new SpellbookService(); // àêî DI íå å àêòèâèðàí
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => View();

		[HttpGet("Talents")]
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
		public IActionResult Consumables()
		{
			// Çàðåæäàìå âñè÷êè ñïåëîâå îò SpellbookService
			var allItems = _spellbookService.GetAllSpells();
			return View(allItems);
		}

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
