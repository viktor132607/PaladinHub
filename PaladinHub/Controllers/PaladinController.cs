using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaladinHub.Models;
using PaladinHub.Models.PageBuilder;
using PaladinHub.Services;
using PaladinHub.Services.PageBuilder;
using PaladinHub.Services.SectionServices;
using PaladinHub.Services.TalentTrees;

namespace PaladinHub.Controllers
{
	[Route("[controller]")]
	public class PaladinController : BaseController
	{
		private readonly BaseSectionService _sectionService;
		private readonly IPageService _pages;
		private readonly ITalentTreeService _talentTrees;

		public PaladinController(
			ISpellbookService spellbookService,
			IItemsService itemsService,
			HolySectionService holyService,
			ProtectionSectionService protectionService,
			RetributionSectionService retributionService,
			IPageService pages,
			IHttpContextAccessor httpContextAccessor,
			ITalentTreeService talentTrees
		)
			: base(
				spellbookService,
				itemsService,
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService)
			)
		{
			_pages = pages;
			_sectionService = PickSectionService(httpContextAccessor, holyService, protectionService, retributionService);
			_talentTrees = talentTrees;
		}

		private static BaseSectionService PickSectionService(
			IHttpContextAccessor accessor,
			HolySectionService holy,
			ProtectionSectionService prot,
			RetributionSectionService retri)
		{
			var http = accessor.HttpContext;
			string? section =
				http?.GetRouteData()?.Values.TryGetValue("section", out var rv) == true ? rv?.ToString() :
				http?.Request?.Query["section"].FirstOrDefault() ??
				http?.Session?.GetString("current-section") ??
				"Retribution";

			section = (section ?? "Retribution").Trim();
			var sectionLower = section.ToLowerInvariant();
			try { http?.Session?.SetString("current-section", sectionLower); } catch { }
			return sectionLower switch
			{
				"holy" => holy,
				"protection" => prot,
				"retribution" => retri,
				_ => retri
			};
		}

		[HttpGet("")]
		public IActionResult Index()
			=> RedirectToAction("Merchandise", "Merchandise");

		private void PutHeaderToViewData(string actionName)
		{
			var section = RouteData.Values.TryGetValue("section", out var s) ? s?.ToString() : null;
			ViewData["Section"] = section;
			ViewData["PageTitle"] = _sectionService.GetPageTitle(actionName);
			ViewData["PageText"] = _sectionService.GetPageText(actionName);
			ViewData["CoverImage"] = _sectionService.GetCoverImage();
			ViewData["CurrentSectionButtons"] = _sectionService.GetCurrentSectionButtons(actionName);
			ViewData["OtherSectionButtons"] = _sectionService.GetOtherSectionButtons();
		}

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Overview")]
		public IActionResult Overview([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Overview));
			return ViewWithCombinedData("Overview");
		}

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Gear")]
		public IActionResult Gear([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Gear));
			return ViewWithCombinedData("Gear");
		}

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Stats")]
		public IActionResult Stats([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Stats));
			return ViewWithCombinedData("Stats");
		}

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Rotation")]
		public IActionResult Rotation([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Rotation));
			return ViewWithCombinedData("Rotation");
		}

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Consumables")]
		public IActionResult Consumables([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Consumables));
			return ViewWithCombinedData("Consumables");
		}

		// Talents: пълним TalentTrees + подаваме ViewData["Spells"] за паршълите
		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Talents")]
		public async Task<IActionResult> Talents([FromRoute] string section)
		{
			PutHeaderToViewData(nameof(Talents));

			// 1) Зареждаме базовите данни
			var spells = await SpellbookService.GetAllAsync();
			var items = await ItemsService.GetAllAsync();

			// 2) Секцията (за да изберем правилните дървета)
			var sec = (section ?? "Retribution").Trim().ToLowerInvariant();

			// 3) Генерираме дърветата (вече на база наличните spells)
			var trees = await _talentTrees.GetTalentTrees(sec, spells);

			// 4) Строим модела (и Page Header-ът остава идентичен)
			var model = new CombinedViewModel
			{
				Spells = spells,
				Items = items,
				TalentTrees = trees,
				PageTitle = _sectionService.GetPageTitle(nameof(Talents)),
				PageText = _sectionService.GetPageText(nameof(Talents)),
				CoverImage = _sectionService.GetCoverImage(),
				CurrentSectionButtons = _sectionService.GetCurrentSectionButtons(nameof(Talents)),
				OtherSectionButtons = _sectionService.GetOtherSectionButtons()
			};

			// Някои вюта четат ViewData["Spells"]
			ViewData["Spells"] = spells;

			return View("Talents", model);
		}

		[AllowAnonymous]
		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/{slug:regex(^(?!Overview$|Gear$|Talents$|Consumables$|Rotation$|Stats$).+)}")]
		public async Task<IActionResult> Page([FromRoute] string section, [FromRoute] string slug)
		{
			var page = await _pages.GetByRouteAsync(section, slug);
			if (page == null || !page.IsPublished) return NotFound();

			var vm = new ContentPageViewModel
			{
				Id = page.Id,
				Section = char.ToUpperInvariant(page.Section[0]) + page.Section[1..],
				Slug = page.Slug,
				Title = page.Title,
				JsonLayout = page.JsonLayout,
				IsPublished = page.IsPublished,
				UpdatedAt = page.UpdatedAt,
				UpdatedBy = page.UpdatedBy,
				RowVersionBase64 = Convert.ToBase64String(page.RowVersion ?? Array.Empty<byte>()),
			};

			return View("ContentPage", vm);
		}
	}
}
