using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
    public class HolyController : Controller
    {
        private readonly ILogger<HolyController> _logger;

        public HolyController(ILogger<HolyController> logger)
        {
            _logger = logger;
        }

        [Route("HolyPaladinGuide")]
        public IActionResult HolyPaladinguide()
        {
            return View();
        }




		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
