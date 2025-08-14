using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using PaladinHub.Models;
using PaladinHub.Models.PageBuilder;      // <-- VM за ContentPage
using PaladinHub.Models.Products;
using PaladinHub.Services.IService;
using PaladinHub.Services.PageBuilder;    // <-- IPageService
using PaladinHub.Services.Products;
using PaladinHub.Services.SectionServices;
using System.Security.Claims;
using System;

namespace PaladinHub.Controllers
{
	public class PaladinController : BaseController
	{
		private readonly BaseSectionService _sectionService;
		private readonly IProductService productService;
		private readonly IPageService _pages;   // динамични CMS страници

		public PaladinController(
			IProductService productService,
			ISpellbookService spellbookService,
			IItemsService itemsService,
			HolySectionService holyService,
			ProtectionSectionService protectionService,
			RetributionSectionService retributionService,
			IPageService pages,
			IHttpContextAccessor httpContextAccessor)
			: base(
				spellbookService,
				itemsService,
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService)
			)
		{
			this.productService = productService;
			_pages = pages;
			_sectionService = (BaseSectionService)
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService);
		}

		private static ISectionService PickSectionService(
			IHttpContextAccessor accessor,
			HolySectionService holy,
			ProtectionSectionService prot,
			RetributionSectionService retri)
		{
			var section = accessor.HttpContext?
								  .GetRouteData()?
								  .Values["section"]?
								  .ToString();

			return section switch
			{
				"Holy" => holy,
				"Protection" => prot,
				"Retribution" => retri,
				_ => throw new ArgumentException($"Unknown section: '{section ?? "(null)"}'")
			};
		}

		[HttpGet]
		public IActionResult Index()
			=> RedirectToAction("Merchandise", "Merchandise");

		// ===== PRODUCTS: CREATE =====
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new CreateProductViewModel();
			var categories = await productService.GetCategories();
			model.CategorySelectList = categories.Select(c => new SelectListItem { Value = c, Text = c });
			model.Images ??= new();
			if (model.Images.Count == 0)
				model.Images.Add(new ProductImageInputModel { Url = "", SortOrder = 0 });
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
				var cats = await productService.GetCategories();
				model.CategorySelectList = cats.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			var created = await productService.Create(model);
			if (created == null)
			{
				ModelState.AddModelError(string.Empty, "Product with this name already exists.");
				var cats = await productService.GetCategories();
				model.CategorySelectList = cats.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			TempData["CreatedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// ===== PRODUCTS: EDIT =====
		[HttpGet]
		public async Task<IActionResult> Edit(string id, CancellationToken ct)
		{
			var vm = await productService.GetForEditAsync(id, ct);
			if (vm == null) return NotFound();

			var categories = await productService.GetCategories();
			vm.CategorySelectList = categories.Select(c => new SelectListItem
			{
				Value = c,
				Text = c,
				Selected = c == vm.Category
			});

			vm.Images ??= new();
			if (vm.Images.Count == 0)
				vm.Images.Add(new ProductImageInputModel { Url = "", SortOrder = 0 });

			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditProductViewModel model, CancellationToken ct)
		{
			if (!ModelState.IsValid)
			{
				var cats = await productService.GetCategories();
				model.CategorySelectList = cats.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			var ok = await productService.UpdateAsync(model, ct);
			if (!ok)
			{
				ModelState.AddModelError(string.Empty, "Name already exists or product not found.");
				var cats = await productService.GetCategories();
				model.CategorySelectList = cats.Select(c => new SelectListItem { Value = c, Text = c });
				return View(model);
			}

			TempData["UpdatedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// ===== PRODUCTS: DELETE =====
		[HttpGet]
		public async Task<IActionResult> DeleteProduct(string id, CancellationToken ct)
		{
			var ok = await productService.Delete(id);
			if (ok) TempData["DeletedSuccessfully"] = true;
			return RedirectToAction("Merchandise", "Merchandise");
		}

		// ===== PRODUCTS: DETAILS =====
		[HttpGet]
		public async Task<IActionResult> Details(string id, CancellationToken ct)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var isAdmin = User.IsInRole("Admin");
			var vm = await productService.GetDetailsAsync(id, userId, isAdmin, ct);
			if (vm == null) return NotFound();
			return View(vm);
		}

		// ===== REVIEWS =====
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddReview(AddReviewInput input, CancellationToken ct)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			if (!ModelState.IsValid)
				return RedirectToAction(nameof(Details), new { id = input.ProductId });

			var ok = await productService.AddReviewAsync(input, userId, ct);
			if (!ok) TempData["ImgError"] = "You can review only products you have in your cart (temporary rule).";

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

		// ===== SECTION STATIC PAGES =====
		private CombinedViewModel BuildSectionModel(string actionName)
		{
			var section = RouteData.Values.TryGetValue("section", out var s) ? s?.ToString() : null;

			var vm = new CombinedViewModel
			{
				Section = section,
				PageTitle = _sectionService.GetPageTitle(actionName),
				PageText = _sectionService.GetPageText(actionName),
				CoverImage = _sectionService.GetCoverImage(),
				CurrentSectionButtons = _sectionService.GetCurrentSectionButtons(actionName),
				OtherSectionButtons = _sectionService.GetOtherSectionButtons()
			};
			return vm;
		}

		// Явни рутове за статичните страници
		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Overview")]
		public IActionResult Overview([FromRoute] string section)
			=> View("Overview", BuildSectionModel(nameof(Overview)));

		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Gear")]
		public IActionResult Gear([FromRoute] string section)
			=> View("Gear", BuildSectionModel(nameof(Gear)));

		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Stats")]
		public IActionResult Stats([FromRoute] string section)
			=> View("Stats", BuildSectionModel(nameof(Stats)));

		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Rotation")]
		public IActionResult Rotation([FromRoute] string section)
			=> View("Rotation", BuildSectionModel(nameof(Rotation)));

		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Consumables")]
		public IActionResult Consumables([FromRoute] string section)
			=> View("Consumables", BuildSectionModel(nameof(Consumables)));

		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/Talents")]
		public IActionResult Talents([FromRoute] string section)
			=> View("Talents", BuildSectionModel(nameof(Talents)));

		// ===== DYNAMIC CMS PAGE =====
		// Всички останали слъгове → CMS (изключваме статичните)
		[AllowAnonymous]
		[HttpGet("{section:regex(^Holy|Protection|Retribution$)}/{slug:regex(^(?!Overview$|Gear$|Talents$|Consumables$|Rotation$|Stats$).+)}")]
		public async Task<IActionResult> Page([FromRoute] string section, [FromRoute] string slug)
		{
			var page = await _pages.GetByRouteAsync(section, slug);
			if (page == null || !page.IsPublished) return NotFound();

			// entity -> VM, който очаква view-то
			var vm = new ContentPageViewModel
			{
				Id = page.Id,
				Section = char.ToUpperInvariant(page.Section[0]) + page.Section[1..],
				Slug = page.Slug,
				Title = page.Title,
				JsonLayout = page.JsonLayout,
				IsPublished = page.IsPublished,
				UpdatedAt = page.UpdatedAt,
				UpdatedBy = page.UpdatedBy,
				RowVersionBase64 = Convert.ToBase64String(page.RowVersion ?? Array.Empty<byte>()),
			};

			return View("ContentPage", vm);
		}
	}
}
