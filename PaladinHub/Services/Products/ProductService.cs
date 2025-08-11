using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;   // Product, User, Cart
using PaladinHub.Data.Models;     // CartProduct, ProductReview, ProductImage
using PaladinHub.Models;          // PagedResult<T>
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Products
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext context;
		public ProductService(AppDbContext context) => this.context = context;

		public async Task<ICollection<ProductViewModel>> GetAll()
			=> await context.Products.AsNoTracking()
				.Select(x => new ProductViewModel
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					ImageUrl = x.ImageUrl,
					Category = x.Category,
					Description = x.Description
				})
				.ToListAsync();

		public async Task<CreateProductViewModel> Create(CreateProductViewModel model)
		{
			if (await context.Products.AnyAsync(x => x.Name == model.Name)) return null!;

			var entity = new Product(model.Name, model.Price)
			{
				ImageUrl = model.ImageUrl,
				Category = model.Category,
				Description = model.Description
			};

			await context.Products.AddAsync(entity);
			await context.SaveChangesAsync();

			if (model.Images != null && model.Images.Count > 0)
			{
				foreach (var img in model.Images.Where(i => !string.IsNullOrWhiteSpace(i.Url)))
				{
					context.ProductImages.Add(new ProductImage
					{
						ProductId = entity.Id,
						Url = img.Url!.Trim(),
						SortOrder = img.SortOrder
					});
				}
				await context.SaveChangesAsync();
			}

			return model;
		}

		public async Task<MyCartViewModel> GetMyProducts(User user)
		{
			var myCartProducts = await context.CartProduct
				.Include(x => x.Product).Include(x => x.Cart)
				.Where(x => x.CartId == user.CartId)
				.ToListAsync();

			var vm = new MyCartViewModel();
			foreach (var cp in myCartProducts)
			{
				if (!vm.MyProducts.Any(x => x.Id == cp.ProductId))
				{
					vm.MyProducts.Add(new ProductViewModel
					{
						Id = cp.ProductId,
						Name = cp.Product.Name,
						Price = cp.Product.Price,
						ImageUrl = cp.Product.ImageUrl,
						Category = cp.Product.Category,
						Description = cp.Product.Description,
						Quantity = cp.Quantity,
						CartId = cp.CartId,
						Cart = cp.Cart
					});
				}
				vm.TotalPrice += cp.Product.Price * cp.Quantity;
			}
			return vm;
		}

		public async Task<bool> Delete(string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return false;
			var entity = await context.Products.FindAsync(id);
			if (entity == null) return false;
			context.Products.Remove(entity);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<List<string>> GetAllCategoriesAsync(CancellationToken ct = default)
			=> await context.Products.AsNoTracking()
				.Select(p => p.Category).Where(c => !string.IsNullOrWhiteSpace(c))
				.Distinct().OrderBy(c => c).ToListAsync(ct);

		public async Task<List<string>> GetCategories()
			=> await context.Products.AsNoTracking()
				.Select(p => p.Category).Where(c => !string.IsNullOrWhiteSpace(c))
				.Distinct().OrderBy(c => c).ToListAsync();

		// Клас за join-а с агрегатите – за да избегнем dynamic в expression trees
		private sealed class AggRow
		{
			public Product P { get; set; } = default!;
			public double Avg { get; set; }
			public int Cnt { get; set; }
		}

		public async Task<PagedResult<ProductListItem>> QueryAsync(ProductQueryOptions options, CancellationToken ct = default)
		{
			IQueryable<Product> baseQ = context.Products.AsNoTracking();

			if (options.MinPrice.HasValue && options.MaxPrice.HasValue &&
				options.MaxPrice.Value < options.MinPrice.Value)
			{
				(options.MinPrice, options.MaxPrice) = (options.MaxPrice, options.MinPrice);
			}

			if (!string.IsNullOrWhiteSpace(options.Search))
			{
				var s = options.Search.Trim();
				baseQ = baseQ.Where(p =>
					EF.Functions.ILike(p.Name, $"%{s}%") ||
					(p.Description != null && EF.Functions.ILike(p.Description, $"%{s}%")) ||
					(p.Category != null && EF.Functions.ILike(p.Category, $"%{s}%"))
				);
			}

			if (options.Categories is { Count: > 0 })
			{
				var cats = options.Categories.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
				if (cats.Count > 0) baseQ = baseQ.Where(p => cats.Contains(p.Category));
			}

			if (options.MinPrice.HasValue) baseQ = baseQ.Where(p => p.Price >= options.MinPrice.Value);
			if (options.MaxPrice.HasValue) baseQ = baseQ.Where(p => p.Price <= options.MaxPrice.Value);

			var agg = context.ProductReviews
				.GroupBy(r => r.ProductId)
				.Select(g => new { ProductId = g.Key, Avg = g.Average(x => (double)x.Rating), Cnt = g.Count() });

			var withAgg =
				from p in baseQ
				join a in agg on p.Id equals a.ProductId into gj
				from a in gj.DefaultIfEmpty()
				select new AggRow
				{
					P = p,
					Avg = (double?)a.Avg ?? 0.0,
					Cnt = (int?)a.Cnt ?? 0
				};

			// Rating range: 1 → [1.00..1.49], 2 → [2.00..2.49], …, 5 → [5.00..5.00]
			if (options.MinRating is int minR && minR >= 1 && minR <= 5)
			{
				double lower = minR;                         // 4 → 4.00
				double upper = (minR < 5) ? minR + 0.49 : 5; // 4 → 4.49, 5 → 5.00
				withAgg = withAgg.Where(x => x.Avg >= lower && x.Avg <= upper);
			}



			IOrderedQueryable<AggRow> ordered = options.SortBy switch
			{
				ProductSortBy.Price =>
					options.Desc ? withAgg.OrderByDescending(x => x.P.Price).ThenBy(x => x.P.Name)
								 : withAgg.OrderBy(x => x.P.Price).ThenBy(x => x.P.Name),

				ProductSortBy.Newest =>
					options.Desc ? withAgg.OrderByDescending(x => x.P.Id)
								 : withAgg.OrderBy(x => x.P.Id),

				ProductSortBy.Name =>
					options.Desc ? withAgg.OrderByDescending(x => x.P.Name)
								 : withAgg.OrderBy(x => x.P.Name),

				ProductSortBy.Rating =>
					options.Desc ? withAgg.OrderByDescending(x => x.Avg).ThenByDescending(x => x.Cnt).ThenBy(x => x.P.Name)
								 : withAgg.OrderBy(x => x.Avg).ThenBy(x => x.P.Name),

				ProductSortBy.MostReviewed =>
					options.Desc ? withAgg.OrderByDescending(x => x.Cnt).ThenByDescending(x => x.Avg).ThenBy(x => x.P.Name)
								 : withAgg.OrderBy(x => x.Cnt).ThenBy(x => x.P.Name),

				_ =>
					options.Desc ? withAgg.OrderByDescending(x => x.P.Name).ThenByDescending(x => x.P.Id)
								 : withAgg.OrderBy(x => x.P.Name).ThenBy(x => x.P.Id)
			};

			var total = await ordered.CountAsync(ct);

			var pageSize = Math.Clamp(options.PageSize, 1, 200);
			var page = Math.Max(1, options.Page);
			var skip = (page - 1) * pageSize;

			var items = await ordered
				.Skip(skip).Take(pageSize)
				.Select(x => new ProductListItem
				{
					Id = x.P.Id,
					Name = x.P.Name,
					Price = x.P.Price,
					ImageUrl = x.P.ImageUrl,
					Category = x.P.Category,
					Description = x.P.Description,
					AverageRating = (decimal)x.Avg,
					ReviewsCount = x.Cnt
				})
				.ToListAsync(ct);

			return new PagedResult<ProductListItem>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};
		}

		public async Task<EditProductViewModel?> GetForEditAsync(string id, CancellationToken ct = default)
			=> await context.Products.AsNoTracking()
				.Where(p => p.Id == id)
				.Select(p => new EditProductViewModel
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					ImageUrl = p.ImageUrl,
					Category = p.Category,
					Description = p.Description
				})
				.FirstOrDefaultAsync(ct);

		public async Task<bool> UpdateAsync(EditProductViewModel model, CancellationToken ct = default)
		{
			var entity = await context.Products.FirstOrDefaultAsync(p => p.Id == model.Id, ct);
			if (entity == null) return false;

			var nameTaken = await context.Products
				.AnyAsync(p => p.Id != model.Id && p.Name == model.Name, ct);
			if (nameTaken) return false;

			entity.Name = model.Name;
			entity.Price = model.Price;
			entity.ImageUrl = model.ImageUrl;
			entity.Category = model.Category;
			entity.Description = model.Description;

			await context.SaveChangesAsync(ct);
			return true;
		}

		public async Task<ProductDetailsViewModel?> GetDetailsAsync(string id, CancellationToken ct)
		{
			var p = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
			if (p == null) return null;

			return new ProductDetailsViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				ImageUrl = p.ImageUrl,
				Category = p.Category,
				Description = p.Description
			};
		}

		public async Task<ProductDetailsViewModel?> GetDetailsAsync(string id, string? currentUserId, bool isAdmin, CancellationToken ct)
		{
			var p = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
			if (p == null) return null;

			var extras = await context.ProductImages
				.Where(i => i.ProductId == id)
				.OrderBy(i => i.SortOrder).ThenBy(i => i.Id)
				.Select(i => new ProductDetailsViewModel.ImageItem { Id = i.Id, Url = i.Url })
				.ToListAsync(ct);

			var images = new List<ProductDetailsViewModel.ImageItem>();
			if (!string.IsNullOrWhiteSpace(p.ImageUrl))
				images.Add(new ProductDetailsViewModel.ImageItem { Id = null, Url = p.ImageUrl! });
			images.AddRange(extras);

			var reviewRows =
				await (from r in context.ProductReviews
					   where r.ProductId == id
					   join u in context.Users on r.UserId equals u.Id into gj
					   from u in gj.DefaultIfEmpty()
					   orderby r.CreatedAt descending
					   select new
					   {
						   r.Id,
						   r.UserId,
						   r.Rating,
						   r.Content,
						   r.CreatedAt,
						   Display = u != null ? (u.Email ?? u.UserName) : r.UserId
					   }).ToListAsync(ct);

			var avg = reviewRows.Count == 0 ? 0 : reviewRows.Average(x => x.Rating);

			var similar = await context.Products.AsNoTracking()
				.Where(x => x.Category == p.Category && x.Id != p.Id)
				.OrderByDescending(x => x.Id)
				.Take(8)
				// ⚠️ ако SimilarVm е глобален (а не вложен) – този конструкт е правилен:
				.Select(x => new SimilarVm
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					ImageUrl = x.ImageUrl
				})
				.ToListAsync(ct);

			return new ProductDetailsViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				ImageUrl = p.ImageUrl,
				Category = p.Category,
				Description = p.Description,
				AverageRating = Math.Round(avg, 1),
				ReviewsCount = reviewRows.Count,
				Reviews = reviewRows.Select(x => new ReviewVm
				{
					Id = x.Id,
					UserName = x.Display,
					Rating = x.Rating,
					Content = x.Content,
					CreatedAt = x.CreatedAt,
					CanDelete = isAdmin || (currentUserId != null && x.UserId == currentUserId)
				}).ToList(),
				Similar = similar,
				Images = images
			};
		}

		public async Task<bool> AddReviewAsync(AddReviewInput input, string userId, CancellationToken ct)
		{
			var hasInCart = await context.CartProduct
				.AnyAsync(cp => cp.ProductId == input.ProductId && cp.Cart.UserId == userId, ct);
			if (!hasInCart) return false;

			var exists = await context.ProductReviews
				.AnyAsync(r => r.ProductId == input.ProductId && r.UserId == userId, ct);
			if (exists) return false;

			var entity = new ProductReview
			{
				ProductId = input.ProductId,
				UserId = userId,
				Rating = input.Rating,
				Content = string.IsNullOrWhiteSpace(input.Content) ? null : input.Content.Trim()
			};

			context.ProductReviews.Add(entity);
			await context.SaveChangesAsync(ct);
			return true;
		}

		public async Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin, CancellationToken ct)
		{
			var r = await context.ProductReviews.FirstOrDefaultAsync(x => x.Id == reviewId, ct);
			if (r == null) return false;
			if (!isAdmin && r.UserId != userId) return false;

			context.ProductReviews.Remove(r);
			await context.SaveChangesAsync(ct);
			return true;
		}

		public async Task<bool> AddImageAsync(string productId, string url, int? sortOrder, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(url)) return false;

			context.ProductImages.Add(new ProductImage
			{
				ProductId = productId,
				Url = url.Trim(),
				SortOrder = sortOrder ?? 0
			});
			await context.SaveChangesAsync(ct);
			return true;
		}

		public async Task<bool> RemoveImageAsync(int imageId, CancellationToken ct)
		{
			var img = await context.ProductImages.FirstOrDefaultAsync(i => i.Id == imageId, ct);
			if (img == null) return false;

			context.ProductImages.Remove(img);
			await context.SaveChangesAsync(ct);
			return true;
		}
	}
}
