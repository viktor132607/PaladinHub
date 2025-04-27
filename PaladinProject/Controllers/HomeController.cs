using Microsoft.AspNetCore.Mvc;
using PaladinProject.Models;
using System.Diagnostics;

namespace PaladinProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("HolyPaladinguide")]
        public IActionResult HolyPaladinguide()
        {
            return View();
        }
        [Route("ProtectionPaladinGuide")]
        public IActionResult ProtectionPaladinguide()
        {
            return View();
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
