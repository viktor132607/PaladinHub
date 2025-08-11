using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Services.Products;
using PaladinHub.Models;
using PaladinHub.Models.Products;

namespace PaladinHub.Controllers
{
	[Route("")]
	[Controller]
	public class MerchandiseController : Controller
	{
		private readonly IProductService _productService;
		private readonly AppDbContext _db;

		public MerchandiseController(IProductService productService, AppDbContext db)
		{
			_productService = productService;
			_db = db;
		}

		[HttpGet("")]
		public IActionResult Index() => RedirectToAction(nameof(Merchandise));

		[HttpGet("Merchandise")]
		public async Task<IActionResult> Merchandise([FromQuery] ProductQueryOptions options, CancellationToken ct = default)
		{
			Normalize(options);

			// основен резултат (услугата вече прави стабилно сортиране;
			// за Rating/MostReviewed добавя вторичен OrderBy по Name)
			var result = await _productService.QueryAsync(options, ct);

			var allCategories = await _productService.GetAllCategoriesAsync(ct);
			var buckets = await BuildRatingBucketsAsync(options, ct);

			var vm = new MerchandisePageViewModel
			{
				Query = options,
				Products = result,
				AllCategories = allCategories,
				RatingAtLeast = buckets
			};

			return View(vm);
		}

		private static void Normalize(ProductQueryOptions o)
		{
			// Page / PageSize
			if (o.Page <= 0) o.Page = 1;
			if (o.PageSize <= 0) o.PageSize = 20;
			if (o.PageSize > 200) o.PageSize = 200;

			// SortBy (ако дойде извън обхвата -> Relevance)
			if (!Enum.IsDefined(typeof(ProductSortBy), o.SortBy))
				o.SortBy = ProductSortBy.Relevance;

			// MinRating в диапазон 1..5
			if (o.MinRating.HasValue)
			{
				if (o.MinRating < 1) o.MinRating = 1;
				if (o.MinRating > 5) o.MinRating = 5;
			}
		}

		/// <summary>
		/// Кумулативни броячи за филтъра „Rating (at least)“.
		/// Прилага Search/Category/Price, но игнорира MinRating.
		/// </summary>
		private async Task<Dictionary<int, int>> BuildRatingBucketsAsync(ProductQueryOptions options, CancellationToken ct)
		{
			// 1) Базова заявка с останалите филтри (без MinRating)
			var q = _db.Products.AsNoTracking().AsQueryable();

			if (!string.IsNullOrWhiteSpace(options.Search))
			{
				var s = options.Search.Trim();
				q = q.Where(p =>
					EF.Functions.ILike(p.Name, $"%{s}%") ||
					(p.Description != null && EF.Functions.ILike(p.Description, $"%{s}%")) ||
					(p.Category != null && EF.Functions.ILike(p.Category, $"%{s}%"))
				);
			}

			if (options.Categories is { Count: > 0 })
			{
				var cats = options.Categories.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
				if (cats.Count > 0)
					q = q.Where(p => cats.Contains(p.Category));
			}

			if (options.MinPrice.HasValue) q = q.Where(p => p.Price >= options.MinPrice.Value);
			if (options.MaxPrice.HasValue) q = q.Where(p => p.Price <= options.MaxPrice.Value);

			// 2) Вземаме средни оценки (Avg) за тези продукти
			var agg = _db.ProductReviews
				.GroupBy(r => r.ProductId)
				.Select(g => new { ProductId = g.Key, Avg = g.Average(x => (double)x.Rating) });

			var withAvg =
				from p in q
				join a in agg on p.Id equals a.ProductId into gj
				from a in gj.DefaultIfEmpty()
				select new { Avg = (double?)a.Avg ?? 0.0 };

			var avgs = await withAvg.ToListAsync(ct);

			// 3) Броим по точни диапазони
			var buckets = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

			foreach (var row in avgs)
			{
				var avg = row.Avg;

				if (avg >= 5.0) buckets[5]++;                        // точно 5.0
				else if (avg >= 4.0 && avg <= 4.49) buckets[4]++;
				else if (avg >= 3.0 && avg <= 3.49) buckets[3]++;
				else if (avg >= 2.0 && avg <= 2.49) buckets[2]++;
				else if (avg >= 1.0 && avg <= 1.49) buckets[1]++;
				// avg < 1.0 => не влиза в никоя кофа
			}

			return buckets;
		}

	}
}
