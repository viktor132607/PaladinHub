using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;
using PaladinHub.Services.Roles;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PaladinHub.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<User> userManager;
		private readonly IProductService productService;
		private readonly IRoleService roleService;

		public HomeController(
			ILogger<HomeController> logger,
			UserManager<User> userManager,
			IProductService productService,
			IRoleService roleService)
		{
			_logger = logger;
			this.userManager = userManager;
			this.productService = productService;
			this.roleService = roleService;
		}

		public IActionResult Home()
		{
			return View();
		}

		public async Task<IActionResult> Merchandise()
		{
			ICollection<ProductViewModel> allProducts = await productService.GetAll();
			return View("Merchandise", allProducts);
		}

		[Authorize]
		public IActionResult ThanksForPurchasing()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> IndexLoggedIn()
		{
			ICollection<ProductViewModel> allProducts = await productService.GetAll();
			return View("Merchandise", allProducts);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Discussion()
		{
			return RedirectToAction("Index", "Discussions");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
