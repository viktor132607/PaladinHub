using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaladinProject.Services;
using PaladinProject.ViewModels;

namespace PaladinProject.Controllers
{
	public class BaseController : Controller
	{
		public ISpellbookService SpellbookService { get; }
		public IItemsService ItemsService { get; }

		public BaseController(ISpellbookService spellbookService, IItemsService itemsService)
		{
			SpellbookService = spellbookService;
			ItemsService = itemsService;
		}

		protected IActionResult ViewWithCombinedData(string viewName = null)
		{
			var controller = RouteData.Values["controller"]?.ToString();

			string? coverImage = controller switch
			{
				"Holy" => "/images/TheHolyCover2.jpg",
				"Protection" => "/images/ProtCoverV2.jpg",
				"Retribution" => "/images/RetributionCoverOrig.jpg",
				_ => null!
			};

			var currentSectionButtons = new List<NavButton>
			{
				new() { Url = $"/{controller}/Overview", Text = "Overview", Icon = "/images/SpellIcons/Divine Hammer.jpg" },
				new() { Url = $"/{controller}/Gear", Text = "BiS Gear", Icon = "/images/itemIcons/inv_chest_plate_earthendungeon_c_01.jpg" },
				new() { Url = $"/{controller}/Talents", Text = "Talent Builds", Icon = "/images/itemIcons/talents.jpg" },
				new() { Url = $"/{controller}/Consumables", Text = "Consumables", Icon = "/images/itemIcons/inv_potion_green.jpg" },
				new() { Url = $"/{controller}/Rotation", Text = "Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" }
			};

			var otherSectionButtons = new List<NavButton>
			{
				new() { Url = $"/{controller}/Overview", Text = "Overview", Icon = "/images/SpellIcons/Divine Hammer.jpg" },
				new() { Url = $"/{controller}/Overview", Text = "Overview", Icon = "/images/SpellIcons/Divine Hammer.jpg" },
				new() { Url = $"/{controller}/Overview", Text = "Overview", Icon = "/images/SpellIcons/Divine Hammer.jpg" },
				new() { Url = $"/{controller}/Gear", Text = "BiS Gear", Icon = "/images/itemIcons/inv_chest_plate_earthendungeon_c_01.jpg" },
				new() { Url = $"/{controller}/Talents", Text = "Talent Builds", Icon = "/images/itemIcons/talents.jpg" },
				new() { Url = $"/{controller}/Consumables", Text = "Consumables", Icon = "/images/itemIcons/inv_potion_green.jpg" },
				new() { Url = $"/{controller}/Rotation", Text = "Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
				new() { Url = $"/{controller}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" }
			};

			var model = new CombinedViewModel
			{
				Spells = SpellbookService.GetAllSpells(),
				Items = ItemsService.GetAllItems(),
				Title = controller ?? "",
				CoverImage = coverImage,
				CurrentSectionButtons = currentSectionButtons,
				OtherSectionButtons = otherSectionButtons
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
