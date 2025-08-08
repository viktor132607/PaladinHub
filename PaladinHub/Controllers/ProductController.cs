using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;

namespace PaladinHub.Controllers
{
	[Authorize] // трябва да си логнат
	public class ProductsController : Controller
	{
		private readonly IProductService productService;

		public ProductsController(IProductService productService)
		{
			this.productService = productService;
		}

		// само Admin може да отваря формата за създаване
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		// само Admin може да създава
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(CreateProductViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var newModel = await productService.Create(model);
			if (newModel == null)
			{
				// продукт с такова име вече съществува
				ModelState.AddModelError(string.Empty, "Product with this name already exists.");
				return View(model);
			}

			// След създаване - връщаме към списъка с продукти
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// само Admin може да трие
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				bool isDeleted = await productService.Delete(id);
				if (isDeleted)
				{
					TempData["DeletedSuccessfully"] = "Deleted";
				}
			}
			return RedirectToAction("Merchandise", "Merchandise");
		}
	}
}
