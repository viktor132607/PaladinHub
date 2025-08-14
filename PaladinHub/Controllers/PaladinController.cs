using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaladinHub.Models;
using PaladinHub.Models.PageBuilder;
using PaladinHub.Services.IService;
using PaladinHub.Services.PageBuilder;
using PaladinHub.Services.SectionServices;
using System;
using System.Linq;

namespace PaladinHub.Controllers
{
	[Route("[controller]")] // Префикс за контролера, за да не хваща корена "/"
	public class PaladinController : BaseController
	{
		private readonly BaseSectionService _sectionService;
		private readonly IPageService _pages;
		private readonly TalentsController _talentsOrchestrator; // оркестратор за Talents

		public PaladinController(
			ISpellbookService spellbookService,
			IItemsService itemsService,
			HolySectionService holyService,
			ProtectionSectionService protectionService,
			RetributionSectionService retributionService,
			IPageService pages,
			IHttpContextAccessor httpContextAccessor,
			TalentsController talentsOrchestrator
		)
			: base(
				spellbookService,
				itemsService,
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService)
			)
		{
			_pages = pages;
			_sectionService = PickSectionService(httpContextAccessor, holyService, protectionService, retributionService);
			_talentsOrchestrator = talentsOrchestrator;
		}

		// По-толерантна резолюция на секцията: route -> query -> session -> default
		private static BaseSectionService PickSectionService(
			IHttpContextAccessor accessor,
			HolySectionService holy,
			ProtectionSectionService prot,
			RetributionSectionService retri)
		{
			var http = accessor.HttpContext;

			// 1) от route: /Paladin/{section}/...
			string? section =
				http?.GetRouteData()?.Values.TryGetValue("section", out var rv) == true ? rv?.ToString() : null
				// 2) от query: ?section=holy
				?? http?.Request?.Query["section"].FirstOrDefault()
				// 3) от сесия, ако вече сме пазили последно избраната
				?? http?.Session?.GetString("current-section")
				// 4) дефолт, ако липсва
				?? "Retribution";

			// нормализиране
			section = (section ?? "Retribution").Trim();
			var sectionLower = section.ToLowerInvariant();

			// запази нормализирания избор за следващи заявки
			try { http?.Session?.SetString("current-section", sectionLower); } catch { /* ignore */ }

			// мапване към сървис
			return sectionLower switch
			{
				"holy" => holy,
				"protection" => prot,
				"retribution" => retri,
				// неизвестно => дефолт (без exception)
				_ => retri
			};
		}

		[HttpGet("")]
		public IActionResult Index()
			=> RedirectToAction("Merchandise", "Merchandise"); // /Paladin

		// ===== SECTION STATIC PAGES =====
		private CombinedViewModel BuildSectionModel(string actionName)
		{
			var section = RouteData.Values.TryGetValue("section", out var s) ? s?.ToString() : null;

			var vm = new CombinedViewModel
			{
				Section = section,
				PageTitle = _sectionService.GetPageTitle(actionName),
				PageText = _sectionService.GetPageText(actionName),
				CoverImage = _sectionService.GetCoverImage(),
				CurrentSectionButtons = _sectionService.GetCurrentSectionButtons(actionName),
				OtherSectionButtons = _sectionService.GetOtherSectionButtons()
			};
			return vm;
		}

		// Абсолютни маршрути (започват с "/"), за да останат на кореново ниво: /Holy/..., /Protection/..., /Retribution/...
		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Overview")]
		public IActionResult Overview([FromRoute] string section)
			=> View("Overview", BuildSectionModel(nameof(Overview)));

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Gear")]
		public IActionResult Gear([FromRoute] string section)
			=> View("Gear", BuildSectionModel(nameof(Gear)));

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Stats")]
		public IActionResult Stats([FromRoute] string section)
			=> View("Stats", BuildSectionModel(nameof(Stats)));

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Rotation")]
		public IActionResult Rotation([FromRoute] string section)
			=> View("Rotation", BuildSectionModel(nameof(Rotation)));

		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Consumables")]
		public IActionResult Consumables([FromRoute] string section)
			=> View("Consumables", BuildSectionModel(nameof(Consumables)));

		// ===== TALENTS (рут тук, логика в оркестратора) =====
		[HttpGet("/{section:regex(^Holy|Protection|Retribution$)}/Talents")]
		public async Task<IActionResult> Talents([FromRoute] string section)
		{
			// 1) Дърветата + базов VM от оркестратора
			var (model, keys, viewPath) = await _talentsOrchestrator.BuildPageAsync(section);

			// 2) Header данни като при останалите секционни страници
			var header = BuildSectionModel(nameof(Talents));
			model.Section = header.Section;
			model.CoverImage = header.CoverImage;
			model.CurrentSectionButtons = header.CurrentSectionButtons;
			model.OtherSectionButtons = header.OtherSectionButtons;

			// 3) Данни за partial-а с 3-те дървета
			ViewData["ShowTalentTrees"] = true;
			ViewData["TalentTreeKeys"] = keys;

			return View(viewPath, model);
		}

		// ===== DYNAMIC CMS PAGE =====
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
