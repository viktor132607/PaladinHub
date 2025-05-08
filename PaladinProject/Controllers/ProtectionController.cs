using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
    public class ProtectionController : Controller
    {
        private readonly ILogger<ProtectionController> _logger;

        public ProtectionController(ILogger<ProtectionController> logger)
        {
            _logger = logger;
        }

        [Route("ProtectionPaladinGuide")]
        public IActionResult ProtectionPaladinguide()
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
