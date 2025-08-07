using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Carts;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Products;
using System.Security.Claims;

namespace PaladinHub.Controllers
{
	[Authorize]
	public class CartsController : Controller
	{
		private ICartService cartService;
		private IProductService productService;
		private UserManager<User> userManager;
		public CartsController(ICartService cartService, IProductService productService, UserManager<User> userManager)
		{
			this.cartService = cartService;
			this.productService = productService;
			this.userManager = userManager;
		}
		public async Task<IActionResult> MyCart()
		{
			User user = await userManager.GetUserAsync(User);
			MyCartViewModel model = await productService.GetMyProducts(user);
			return View(model);
		}

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
			return Redirect("/Home/IndexLoggedIn/");
		}

		public async Task<IActionResult> IncreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool isRemoved = await cartService.IncreaseProduct(id, userId);
				if (isRemoved)
				{
					TempData["AddedSuccessfully"] = "Done";
				}
			}
			return Redirect("/Carts/MyCart");
		}
		public async Task<IActionResult> DecreaseProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				bool isRemoved = await cartService.DecreaseProduct(id, userId);
				if (isRemoved)
				{
					TempData["RemovedFromCart"] = "Decreased";
				}
			}
			return Redirect("/Carts/MyCart");
		}
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
			return Redirect("/Carts/MyCart");
		}

		public async Task<IActionResult> Buy()
		{
			User user = await userManager.GetUserAsync(User);

			await cartService.ArchiveCart(user);

			TempData["Buy"] = "Done";

			return Redirect("/Home/IndexLoggedIn");
			//return Redirect("/Home/ThanksForPurchasing/");
		}

		public async Task<IActionResult> Cancel()
		{
			Data.Entities.User user = await userManager.GetUserAsync(User);

			await cartService.CleanCart(user);

			TempData["Canceled"] = "Canceled";

			return Redirect("/Home/IndexLoggedIn");
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Archive()
		{
			ICollection<CartViewModel> archivedCarts = await cartService.GetArchive();

			return View(archivedCarts);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Details(string id)
		{
			MyCartViewModel cart = await cartService.GetCartById(id);

			return View(cart);
		}
	}
}