using PaladinProject.Services.IService;
using PaladinProject.ViewModels;

namespace PaladinProject.Services.SectionServices
{
	public abstract class BaseSectionService : ISectionService
	{
		public abstract string ControllerName { get; }

		public virtual string GetCoverImage() => null;

		public virtual string GetPageTitle(string actionName) => ControllerName;
		public virtual string GetPageText(string actionName) => null;

		public virtual List<NavButton> GetCurrentSectionButtons(string actionName)
		{
			if (SectionButtonsMap.TryGetValue(actionName, out var buttons))
				return buttons;

			// fallback specific action
			return GetInternalAnchorButtons();
		}

		public virtual List<NavButton> GetOtherSectionButtons() =>
			AllButtons.Where(b => !b.IsAnchor).ToList();

		// anchor buttons for specific sections
		protected virtual List<NavButton> GetInternalAnchorButtons() => new()
		{
			new() { Url = "#rotation", Text = "Scroll to Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg", IsAnchor = true },
			new() { Url = "#talents", Text = "Scroll to Talents", Icon = "/images/itemIcons/talents.jpg", IsAnchor = true }
		};

		// general buttons for all sections
		protected virtual List<NavButton> AllButtons => new()
		{
			new() { Url = $"/{ControllerName}/Overview", Text = "Overview", Icon = "/images/SpellIcons/Divine Hammer.jpg" },
			new() { Url = $"/{ControllerName}/Gear", Text = "BiS Gear", Icon = "/images/itemIcons/inv_chest_plate_earthendungeon_c_01.jpg" },
			new() { Url = $"/{ControllerName}/Talents", Text = "Talent Builds", Icon = "/images/itemIcons/talents.jpg" },
			new() { Url = $"/{ControllerName}/Consumables", Text = "Consumables", Icon = "/images/itemIcons/inv_potion_green.jpg" },
			new() { Url = $"/{ControllerName}/Rotation", Text = "Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg" },
			new() { Url = $"/{ControllerName}/Stats", Text = "Stats", Icon = "/images/icons/inv_10_inscription2_repcontracts_scroll_02_uprez_color2.jpg" },
			new() { Url = $"/{ControllerName}/Overview", Text = "CheatSheet", Icon = "/images/itemIcons/inv_misc_note_03.jpg" },
			new() { Url = $"/{ControllerName}/WA & Addons", Text = "WA & Addons", Icon = "/images/icons/WA.png" },

			// Internal anchor buttons
			new() { Url = "#rotation", Text = "Scroll to Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg", IsAnchor = true },
			new() { Url = "#talents", Text = "Scroll to Talents", Icon = "/images/itemIcons/talents.jpg", IsAnchor = true },
			new() { Url = "#overall-bis", Text = "Enchants", Icon = "/images/itemIcons/inv_misc_enchantedscroll.jpg", IsAnchor = true },
			new() { Url = "#raid-mythic-bis", Text = "Consumables", Icon = "/images/itemIcons/inv_potion_green.jpg", IsAnchor = true },
			new() { Url = "#best-trinkets", Text = "Food", Icon = "/images/itemIcons/inv_misc_food_meat_cooked_02_color02.jpg", IsAnchor = true },
			new() { Url = "#upgrade-priorities", Text = "Gems", Icon = "/images/itemIcons/inv_10_jewelcrafting_gem3primal_cut_red.jpg", IsAnchor = true }
		};


		protected virtual Dictionary<string, List<NavButton>> SectionButtonsMap => new()
		{
			["Consumables"] = new()
			{
				new() { Url = "#overall-bis", Text = "Enchants", Icon = "/images/itemIcons/inv_misc_enchantedscroll.jpg", IsAnchor = true },
				new() { Url = "#raid-mythic-bis", Text = "Consumables", Icon = "/images/itemIcons/inv_potion_green.jpg", IsAnchor = true },
				new() { Url = "#best-trinkets", Text = "Food", Icon = "/images/itemIcons/inv_misc_food_meat_cooked_02_color02.jpg", IsAnchor = true },
				new() { Url = "#upgrade-priorities", Text = "Gems", Icon = "/images/itemIcons/inv_10_jewelcrafting_gem3primal_cut_red.jpg", IsAnchor = true }
			},
			["Rotation"] = new()
			{
				new() { Url = "#rotation", Text = "Scroll to Rotation", Icon = "/images/icons/ui_spellbook_onebutton.jpg", IsAnchor = true }
			},
			["Talents"] = new()
			{
				new() { Url = "#talents", Text = "Scroll to Talents", Icon = "/images/itemIcons/talents.jpg", IsAnchor = true }
			}
		};
	}
}
