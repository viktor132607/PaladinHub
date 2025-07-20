using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaladinProject.Services;
using PaladinProject.ViewModels;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
	[Route("Protection")]
	public class ProtectionController : BaseController
	{
		private readonly ILogger<ProtectionController> _logger;

		public ProtectionController(
			ILogger<ProtectionController> logger,
			ISpellbookService spellbookService,
			IItemsService itemsService
		) : base(spellbookService, itemsService)
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
