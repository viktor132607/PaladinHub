using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Carts;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Products;
using System.Security.Claims;
using System;

namespace PaladinHub.Controllers
{
	[Authorize]
	[Route("Cart")]
	public class CartsController : Controller
	{
		private readonly ICartService cartService;
		private readonly IProductService productService;
		private readonly UserManager<User> userManager;

		public CartsController(
			ICartService cartService,
			IProductService productService,
			UserManager<User> userManager)
		{
			this.cartService = cartService;
			this.productService = productService;
			this.userManager = userManager;
		}

		// Показва количката на текущия потребител (използва ProductService.GetMyProducts(User))
		[HttpGet("MyCart")]
		public async Task<IActionResult> MyCart()
		{
			var user = await userManager.GetUserAsync(User);
			MyCartViewModel model = await productService.GetMyProducts(user);
			return View(model);
		}

		[HttpGet("AddProduct/{id}")]
		public async Task<IActionResult> AddProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool isCreated = await cartService.AddProduct(id, userId);
				if (isCreated)
				{
					TempData["AddedSuccessfully"] = "Done";
				}
			}
			// Връща към списъка със стоки
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet("IncreaseProduct/{id}")]
		public async Task<IActionResult> IncreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool ok = await cartService.IncreaseProduct(id, userId);
				if (ok)
				{
					TempData["AddedSuccessfully"] = "Done";
				}
			}
			return RedirectToAction("MyCart", "Carts");
		}

		[HttpGet("DecreaseProduct/{id}")]
		public async Task<IActionResult> DecreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool ok = await cartService.DecreaseProduct(id, userId);
				if (ok)
				{
					TempData["RemovedFromCart"] = "Removed";
				}
			}
			return RedirectToAction("MyCart", "Carts");
		}

		[HttpGet("RemoveProductFromCart/{id}")]
		public async Task<IActionResult> RemoveProductFromCart(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool isRemoved = await cartService.RemoveProduct(id, userId);
				if (isRemoved)
				{
					TempData["RemovedFromCart"] = "Removed";
				}
			}
			return RedirectToAction("MyCart", "Carts");
		}

		// Купуване = архивира количката на потребителя
		[HttpGet("Buy")]
		public async Task<IActionResult> Buy()
		{
			var user = await userManager.GetUserAsync(User);
			await cartService.ArchiveCart(user);

			TempData["Buy"] = "Done";
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// Отказ = изчиства количката
		[HttpGet("Cancel")]
		public async Task<IActionResult> Cancel()
		{
			var user = await userManager.GetUserAsync(User);
			await cartService.CleanCart(user);

			TempData["Canceled"] = "Done";
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// Админ: списък с архивирани колички
		[Authorize(Roles = "Admin")]
		[HttpGet("Archive")]
		public async Task<IActionResult> Archive()
		{
			ICollection<CartViewModel> archivedCarts = await cartService.GetArchive();
			return View(archivedCarts);
		}

		// Админ: детайли за конкретна количка
		[Authorize(Roles = "Admin")]
		[HttpGet("Details/{id:guid}")]
		public async Task<IActionResult> Details(Guid id)
		{
			var cart = await cartService.GetCartById(id);
			if (cart == null) return NotFound();

			return View(cart);
		}
	}
}