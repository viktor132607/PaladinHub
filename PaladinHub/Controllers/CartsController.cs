using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Carts;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Products;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace PaladinHub.Controllers
{
	[Authorize]
	[Route("Cart")]
	public class CartsController : Controller
	{
		private readonly ICartService cartService;
		private readonly IProductService productService;
		private readonly UserManager<User> userManager;
		private readonly ICartSessionService cartSession;

		public CartsController(
			ICartService cartService,
			IProductService productService,
			UserManager<User> userManager,
			ICartSessionService cartSession)
		{
			this.cartService = cartService;
			this.productService = productService;
			this.userManager = userManager;
			this.cartSession = cartSession;
		}

		[HttpGet("MyCart")]
		public async Task<IActionResult> MyCart()
		{
			User user = await userManager.GetUserAsync(User);
			System.Threading.CancellationToken ct = HttpContext.RequestAborted;
			await cartSession.SyncRedisToPersistent(user, ct);
			MyCartViewModel model = await productService.GetMyProducts(user);
			return View(model);
		}

		[HttpGet("AddProduct/{id}")]
		public async Task<IActionResult> AddProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				System.Threading.CancellationToken ct = HttpContext.RequestAborted;
				bool ok = await cartSession.AddProduct(id, userId, ct);
				if (ok) TempData["AddedSuccessfully"] = "Done";
			}
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet("IncreaseProduct/{id}")]
		public async Task<IActionResult> IncreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				System.Threading.CancellationToken ct = HttpContext.RequestAborted;
				bool ok = await cartSession.IncreaseProduct(id, userId, ct);
				if (ok) TempData["AddedSuccessfully"] = "Done";
			}
			return RedirectToAction("MyCart", "Carts");
		}

		[HttpGet("DecreaseProduct/{id}")]
		public async Task<IActionResult> DecreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				System.Threading.CancellationToken ct = HttpContext.RequestAborted;
				bool ok = await cartSession.DecreaseProduct(id, userId, ct);
				if (ok) TempData["RemovedFromCart"] = "Removed";
			}
			return RedirectToAction("MyCart", "Carts");
		}

		[HttpGet("RemoveProductFromCart/{id}")]
		public async Task<IActionResult> RemoveProductFromCart(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				System.Threading.CancellationToken ct = HttpContext.RequestAborted;
				bool ok = await cartSession.RemoveProduct(id, userId, ct);
				if (ok) TempData["RemovedFromCart"] = "Removed";
			}
			return RedirectToAction("MyCart", "Carts");
		}

		[HttpGet("Buy")]
		public async Task<IActionResult> Buy()
		{
			User user = await userManager.GetUserAsync(User);
			System.Threading.CancellationToken ct = HttpContext.RequestAborted;
			await cartSession.ArchiveAndClear(user, ct);
			TempData["Buy"] = "Done";
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet("Cancel")]
		public async Task<IActionResult> Cancel()
		{
			User user = await userManager.GetUserAsync(User);
			System.Threading.CancellationToken ct = HttpContext.RequestAborted;
			await cartSession.CleanAndClear(user, ct);
			TempData["Canceled"] = "Done";
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet("Mini")]
		public async Task<IActionResult> Mini()
		{
			var user = await userManager.GetUserAsync(User);
			var model = await productService.GetMyProducts(user);
			return PartialView("_MiniCart", model);
		}

		[HttpGet("CountJson")]
		public async Task<IActionResult> CountJson()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Json(0);
			var ct = HttpContext.RequestAborted;
			int count = await cartSession.GetCount(userId, ct);
			return Json(count);
		}



		[Authorize(Roles = "Admin")]
		[HttpGet("Archive")]
		public async Task<IActionResult> Archive()
		{
			System.Collections.Generic.ICollection<CartViewModel> archivedCarts = await cartService.GetArchive();
			return View(archivedCarts);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("Details/{id:guid}")]
		public async Task<IActionResult> Details(Guid id)
		{
			MyCartViewModel cart = await cartService.GetCartById(id);
			if (cart == null) return NotFound();
			return View(cart);
		}
	}
}
