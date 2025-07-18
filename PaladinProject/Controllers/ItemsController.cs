using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;

namespace PaladinProject.Controllers
{
    [Route("Items")]
    public class ItemsController : Controller
    {
        private readonly ItemsService _itemsService;

        public ItemsController()
        {
            _itemsService = new ItemsService();
        }

        [HttpGet("{spec}/Consumables")]
        public IActionResult Consumables(string spec)
        {
            var allItems = _itemsService.GetAllItems();
            return View($"~/Views/{spec}/Consumables.cshtml", allItems);
        }
    }
}
