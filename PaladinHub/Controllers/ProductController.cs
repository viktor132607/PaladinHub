using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PaladinHub.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductService productService;

		public ProductsController(IProductService productService)
		{
			this.productService = productService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new CreateProductViewModel();

			var categories = await productService.GetCategories();
			model.CategorySelectList = categories.Select(c => new SelectListItem { Value = c, Text = c });

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductViewModel model, CancellationToken ct)
		{
	
			if (!string.IsNullOrWhiteSpace(model.NewCategory))
				model.Category = model.NewCategory.Trim();

			if (!ModelState.IsValid)
			{
				// при невалидна форма трябва пак да подадем списъка с категории
				var categories = await productService.GetCategories();
				model.CategorySelectList = categories.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			var created = await productService.Create(model);
			if (created == null)
			{
				ModelState.AddModelError(string.Empty, "Product with this name already exists.");

				var categories = await productService.GetCategories();
				model.CategorySelectList = categories.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			TempData["CreatedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id, CancellationToken ct)
		{
			var vm = await productService.GetForEditAsync(id, ct);
			if (vm == null) return NotFound();
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditProductViewModel model, CancellationToken ct)
		{
			if (!ModelState.IsValid) return View(model);

			var ok = await productService.UpdateAsync(model, ct);
			if (!ok)
			{
				ModelState.AddModelError(string.Empty, "Name already exists or product not found.");
				return View(model);
			}

			TempData["UpdatedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet]
		public async Task<IActionResult> DeleteProduct(string id, CancellationToken ct)
		{
			var ok = await productService.Delete(id);
			if (ok) TempData["DeletedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		[HttpGet]
		public async Task<IActionResult> Details(string id, CancellationToken ct)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var isAdmin = User.IsInRole("Admin");
			var vm = await productService.GetDetailsAsync(id, userId, isAdmin, ct);
			if (vm == null) return NotFound();
			return View(vm);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddReview(AddReviewInput input, CancellationToken ct)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			if (!ModelState.IsValid)
				return RedirectToAction(nameof(Details), new { id = input.ProductId });

			await productService.AddReviewAsync(input, userId, ct);
			return RedirectToAction(nameof(Details), new { id = input.ProductId });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteReview(int id, string productId, CancellationToken ct)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			var isAdmin = User.IsInRole("Admin");
			await productService.DeleteReviewAsync(id, userId, isAdmin, ct);
			return RedirectToAction(nameof(Details), new { id = productId });
		}
	}
}
