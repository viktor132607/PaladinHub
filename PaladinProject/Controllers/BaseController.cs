using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaladinProject.Services.IService;
using PaladinProject.ViewModels;

namespace PaladinProject.Controllers
{
	public abstract class BaseController : Controller
	{
		public ISpellbookService SpellbookService { get; }
		public IItemsService ItemsService { get; }
		public ISectionService SectionService { get; }

		public BaseController(
			ISpellbookService spellbookService,
			IItemsService itemsService,
			ISectionService sectionService)
		{
			SpellbookService = spellbookService;
			ItemsService = itemsService;
			SectionService = sectionService;
		}

		protected IActionResult ViewWithCombinedData(string viewName = null)
		{
			var controller = RouteData.Values["controller"]?.ToString() ?? "";
			var action = RouteData.Values["action"]?.ToString() ?? "";

			var model = new CombinedViewModel
			{
				Spells = SpellbookService.GetAllSpells(),
				Items = ItemsService.GetAllItems(),
				PageTitle = SectionService.GetPageTitle(action),
				PageText = SectionService.GetPageText(action),
				CoverImage = SectionService.GetCoverImage(),
				CurrentSectionButtons = SectionService.GetCurrentSectionButtons(action),
				OtherSectionButtons = SectionService.GetOtherSectionButtons()
			};

			return viewName == null ? View(model) : View(viewName, model);
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			ViewBag.AllSpells = SpellbookService.GetAllSpells();
			ViewBag.AllItems = ItemsService.GetAllItems();
			base.OnActionExecuting(context);
		}
	}
}
