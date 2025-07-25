using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaladinHub.Services.IService;
using PaladinHub.Services.SectionServices;
using PaladinHub.Models;
using System.Diagnostics;

namespace PaladinHub.Controllers
{
	[Route("Holy")]
	public class HolyController : BaseController
	{
		private readonly ILogger<HolyController> _logger;

		public HolyController(
			ILogger<HolyController> logger,
			ISpellbookService spellbookService,
			IItemsService itemsService,
			HolySectionService sectionService)
			: base(spellbookService, itemsService, sectionService)
		{
			_logger = logger;
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => ViewWithCombinedData();

		[HttpGet("Talents")]
		public IActionResult Talents() => ViewWithCombinedData();

		[HttpGet("Stats")]
		public IActionResult Stats() => ViewWithCombinedData();

		[HttpGet("Consumables")]
		public IActionResult Consumables() => ViewWithCombinedData();

		[HttpGet("Gear")]
		public IActionResult Gear() => ViewWithCombinedData();

		[HttpGet("Rotation")]
		public IActionResult Rotation() => ViewWithCombinedData();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() =>
			View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
