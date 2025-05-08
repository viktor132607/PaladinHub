using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
    [Route("Protection")]
    public class ProtectionController : Controller
    {
        private readonly ILogger<ProtectionController> _logger;

        public ProtectionController(ILogger<ProtectionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Overview")]
        public IActionResult Overview() => View();

        [HttpGet("Talents")]
        public IActionResult Talents() => View();

        [HttpGet("Stats")]
        public IActionResult Stats() => View();

        [HttpGet("Consumables")]
        public IActionResult Consumables() => View();

        [HttpGet("Gear")]
        public IActionResult Gear() => View();

        [HttpGet("Rotation")]
        public IActionResult Rotation() => View();





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
