using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models;
using PaladinHub.Models.Products;
using PaladinHub.Services.Products;
using System.Threading;

namespace PaladinHub.Controllers
{
	[Route("[controller]")]
	public class MerchandiseController : Controller
	{
		private readonly IProductService productService;

		public MerchandiseController(IProductService productService)
		{
			this.productService = productService;
		}

		[HttpGet("Merchandise")]
		public async Task<IActionResult> Merchandise([FromQuery] ProductQueryOptions options, CancellationToken ct)
		{
			if (options.PageSize <= 0) options.PageSize = 40;

			var result = await productService.QueryAsync(options, ct);
			var allCategories = await productService.GetAllCategoriesAsync(ct);

			var vm = new MerchandisePageViewModel
			{
				Query = options,
				Products = result,
				AllCategories = allCategories
			};

			return View(vm);
		}
	}
}
