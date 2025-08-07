using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;

namespace PaladinHub.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		private IProductService productService;

		public ProductsController(IProductService productService)
		{
			this.productService = productService;
		}
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateProductViewModel model)
		{
			if (ModelState.IsValid)
			{
				CreateProductViewModel newModel = await productService.Create(model);
				if (newModel == null)
				{
					return View(model);
				}
				return Redirect("/Home/IndexLoggedIn/");
			}
			return View(model);
		}

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
			return Redirect("/Home/IndexLoggedIn/");
		}
	}
}