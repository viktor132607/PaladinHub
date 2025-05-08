using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
    public class RetributionController : Controller
    {
        private readonly ILogger<RetributionController> _logger;

        public RetributionController(ILogger<RetributionController> logger)
        {
            _logger = logger;
        }

       [Route("RetributionPaladinGuide")]
        public IActionResult RetributionPaladinGuide()
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
