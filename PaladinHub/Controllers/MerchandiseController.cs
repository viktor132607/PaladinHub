using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;

namespace PaladinHub.Controllers
{
	public class MerchandiseController : Controller
	{
		private readonly IProductService productService;

		public MerchandiseController(IProductService productService)
		{
			this.productService = productService;
		}

		public async Task<IActionResult> Merchandise()
		{
			ICollection<ProductViewModel> products = await productService.GetAll();
			return View("Merchandise", products);
		}
	}
}
