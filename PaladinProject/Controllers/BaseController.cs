using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;

namespace PaladinProject.Controllers
{
    public class BaseController : Controller
    {
        protected readonly SpellbookService _spellbookService;
        protected readonly ItemsService _itemService;

        public BaseController()
        {
            _spellbookService = new SpellbookService();
            _itemService = new ItemsService();
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            ViewBag.AllSpells = _spellbookService.GetAllSpells();
            ViewBag.AllItems = _itemService.GetAllItems();
            base.OnActionExecuting(context);
        }
    }
}
