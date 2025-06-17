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
		private readonly ILogger<ProtectionController> _logger;
		private readonly SpellbookContext _context;

		public ProtectionController(ILogger<ProtectionController> logger, SpellbookContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpGet("Overview")]
		public IActionResult Overview()
		{
			var model = BuildModel("Overview content goes here...");
			return View(model);
		}

		[HttpGet("Talents")]
		public IActionResult Talents()
		{
			var model = BuildModel("Talents description content goes here...");
			return View(model);
		}

		[HttpGet("Stats")]
		public IActionResult Stats()
		{
			var model = BuildModel("Stats description content goes here...");
			return View(model);
		}

		[HttpGet("Consumables")]
		public IActionResult Consumables()
		{
			var model = BuildModel("Consumables description content goes here...");
			return View(model);
		}

		[HttpGet("Gear")]
		public IActionResult Gear()
		{
			var model = BuildModel("Gear description content goes here...");
			return View(model);
		}

		[HttpGet("Rotation")]
		public IActionResult Rotation()
		{
			var model = BuildModel("Rotation guide content goes here...");
			return View(model);
		}

		private TalentsViewModel BuildModel(string description)
		{
			return new TalentsViewModel
			{
				Description = description,
				SpellNames = _context.Spells.Select(s => s.Name).ToList(),
				SpellBook = new SpellBook()
			};
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			});
		}
	}
}
