using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Carts;
using PaladinHub.Services.Carts;
using PaladinHub.Services.Products;

namespace PaladinHub.Controllers
{
	[Authorize]
	[Route("Cart")]
	[AutoValidateAntiforgeryToken]
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

		// ===== Helpers =====

		private string? CurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

		private async Task<User?> CurrentUserAsync() => await userManager.GetUserAsync(User);

		/// <summary>Връща текущата количка като ViewModel (и синхронизира Redis→DB при нужда).</summary>
		private async Task<MyCartViewModel> GetCartVmAsync(User user, CancellationToken ct)
		{
			await cartSession.SyncRedisToPersistent(user, ct);
			return await productService.GetMyProducts(user);
		}

		private IActionResult AjaxError() => Json(new { ok = false });

		// ===== Views =====

		[HttpGet("MyCart")]
		public async Task<IActionResult> MyCart()
		{
			var user = await CurrentUserAsync();
			if (user == null) return Challenge();

			var ct = HttpContext.RequestAborted;
			var model = await GetCartVmAsync(user, ct);
			return View(model);
		}

		// ===== Add from catalog (redirect UX остава) =====

		[HttpGet("AddProduct/{id}")]
		public async Task<IActionResult> AddProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var userId = CurrentUserId();
				var ct = HttpContext.RequestAborted;
				if (userId != null)
				{
					var ok = await cartSession.AddProduct(id, userId, ct);
					if (ok) TempData["AddedSuccessfully"] = "Done";
				}
			}
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// ===== AJAX: Increase / Decrease / Remove =====

		[HttpPost("Increase")]
		public async Task<IActionResult> Increase(string id)
		{
			var userId = CurrentUserId();
			if (string.IsNullOrWhiteSpace(id) || userId == null) return AjaxError();

			var ct = HttpContext.RequestAborted;
			var ok = await cartSession.IncreaseProduct(id, userId, ct);
			if (!ok) return AjaxError();

			return await CartDeltaJsonAsync(id, removed: false, ct);
		}

		[HttpPost("Decrease")]
		public async Task<IActionResult> Decrease(string id)
		{
			var userId = CurrentUserId();
			if (string.IsNullOrWhiteSpace(id) || userId == null) return AjaxError();

			var ct = HttpContext.RequestAborted;
			var ok = await cartSession.DecreaseProduct(id, userId, ct);
			if (!ok) return AjaxError();

			// ако количеството е станало 0 – редът вече не присъства в VM и ще се отчете като removed
			return await CartDeltaJsonAsync(id, removed: null, ct);
		}

		[HttpPost("RemoveProduct")]
		public async Task<IActionResult> RemoveProduct(string id)
		{
			var userId = CurrentUserId();
			if (string.IsNullOrWhiteSpace(id) || userId == null) return AjaxError();

			var ct = HttpContext.RequestAborted;
			var ok = await cartSession.RemoveProduct(id, userId, ct);
			if (!ok) return AjaxError();

			return await CartDeltaJsonAsync(id, removed: true, ct);
		}

		/// <summary>
		/// Връща JSON за обновяване на ред + тотал след промяна.
		/// removed:
		///   - true  -> принудително маркиране като изтрит
		///   - false -> гарантирано наличен
		///   - null  -> определя се според това дали го намираме в модела
		/// </summary>
		private async Task<IActionResult> CartDeltaJsonAsync(string productId, bool? removed, CancellationToken ct)
		{
			var user = await CurrentUserAsync();
			if (user == null) return AjaxError();

			var vm = await GetCartVmAsync(user, ct);

			// FIX: MyProducts е ICollection -> ползваме LINQ вместо .Find()
			var item = vm.MyProducts.FirstOrDefault(p => p.Id == productId);
			var isRemoved = removed ?? (item == null);

			double unit = item != null ? (double)item.Price : 0.0;
			int qty = item != null ? (item.Quantity > 0 ? item.Quantity : 0) : 0;
			double line = item != null ? (double)(item.Price * (qty == 0 ? 1 : qty)) : 0.0;
			double cart = (double)vm.TotalPrice;

			return Json(new
			{
				ok = true,
				productId = productId,
				removed = isRemoved,
				quantity = qty,
				unitPrice = unit,
				lineTotal = isRemoved ? 0.0 : line,
				cartTotal = cart
			});
		}

		// ===== AJAX: Buy / Cancel =====

		[HttpPost("Buy")]
		public async Task<IActionResult> Buy()
		{
			var user = await CurrentUserAsync();
			if (user == null) return AjaxError();

			var ct = HttpContext.RequestAborted;
			await cartSession.ArchiveAndClear(user, ct);

			return Json(new
			{
				ok = true,
				cleared = true,
				cartTotal = 0.0,
				message = "Purchase completed successfully."
			});
		}

		[HttpPost("Cancel")]
		public async Task<IActionResult> Cancel()
		{
			var user = await CurrentUserAsync();
			if (user == null) return AjaxError();

			var ct = HttpContext.RequestAborted;
			await cartSession.CleanAndClear(user, ct);

			return Json(new
			{
				ok = true,
				cleared = true,
				cartTotal = 0.0,
				message = "Cart was cleared."
			});
		}

		// ===== Mini cart / count =====

		[HttpGet("Mini")]
		public async Task<IActionResult> Mini()
		{
			var user = await CurrentUserAsync();
			if (user == null) return Unauthorized();

			var model = await productService.GetMyProducts(user);
			return PartialView("_MiniCart", model);
		}

		[HttpGet("CountJson")]
		public async Task<IActionResult> CountJson()
		{
			var userId = CurrentUserId();
			if (string.IsNullOrEmpty(userId)) return Json(0);
			var ct = HttpContext.RequestAborted;
			var count = await cartSession.GetCount(userId, ct);
			return Json(count);
		}

		// ===== Admin =====

		[Authorize(Roles = "Admin")]
		[HttpGet("Archive")]
		public async Task<IActionResult> Archive()
		{
			var archivedCarts = await cartService.GetArchive();
			return View(archivedCarts);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("Details/{id:guid}")]
		public async Task<IActionResult> Details(System.Guid id)
		{
			var cart = await cartService.GetCartById(id);
			if (cart == null) return NotFound();
			return View(cart);
		}
	}
}
