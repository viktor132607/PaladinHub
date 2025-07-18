using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;

namespace PaladinProject.Controllers
{
    [Route("Spellbook")]
    public class SpellbookController : Controller
    {
        private readonly SpellbookService _spellbookService;

        public SpellbookController()
        {
            _spellbookService = new SpellbookService();
        }

        [HttpGet("{spec}/Talents")]
        public IActionResult Talents(string spec)
        {
            var allSpells = _spellbookService.GetAllSpells();
            return View($"~/Views/{spec}/Talents.cshtml", allSpells);
        }

        [HttpGet("{spec}/Stats")]
        public IActionResult Stats(string spec)
        {
            var allSpells = _spellbookService.GetAllSpells();
            return View($"~/Views/{spec}/Stats.cshtml", allSpells);
        }
    }
}
