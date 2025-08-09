using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models;
using PaladinHub.Services.IService;

namespace PaladinHub.Controllers
{
	public abstract class BaseController : Controller
	{
		protected ISpellbookService SpellbookService { get; }
		protected IItemsService ItemsService { get; }
		protected ISectionService SectionService { get; }

		protected BaseController(
			ISpellbookService spellbookService,
			IItemsService itemsService,
			ISectionService sectionService)
		{
			SpellbookService = spellbookService;
			ItemsService = itemsService;
			SectionService = sectionService;
		}

		protected async Task<IActionResult> ViewWithCombinedDataAsync(string? viewName = null)
		{
			var action = RouteData.Values["action"]?.ToString() ?? "";

			var model = new CombinedViewModel
			{
				Spells = await SpellbookService.GetAllAsync(),
				Items = await ItemsService.GetAllAsync(),
				PageTitle = SectionService.GetPageTitle(action),
				PageText = SectionService.GetPageText(action),
				CoverImage = SectionService.GetCoverImage(),
				CurrentSectionButtons = SectionService.GetCurrentSectionButtons(action),
				OtherSectionButtons = SectionService.GetOtherSectionButtons()
			};

			return viewName is null ? View(model) : View(viewName, model);
		}

		protected IActionResult ViewWithCombinedData(string? viewName = null)
		{
			return ViewWithCombinedDataAsync(viewName).GetAwaiter().GetResult();
		}
	}
}
