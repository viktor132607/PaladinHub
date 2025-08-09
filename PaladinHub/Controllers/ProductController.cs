using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;

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
		public IActionResult Create()
		{
			return View(new CreateProductViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductViewModel model, CancellationToken ct)
		{
			if (!ModelState.IsValid) return View(model);

			var created = await productService.Create(model);
			if (created == null)
			{
				ModelState.AddModelError(string.Empty, "Product with this name already exists.");
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
	}
}
