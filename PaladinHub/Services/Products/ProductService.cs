using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;   // Product, User, Cart
using PaladinHub.Data.Models;     // CartProduct, ProductReview, ProductImage
using PaladinHub.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Products
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext context;

		public ProductService(AppDbContext context) => this.context = context;

		// ==== BASIC LIST ====
		public async Task<ICollection<ProductViewModel>> GetAll()
		{
			return await context.Products
				.AsNoTracking()
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
		}

		// ==== CREATE PRODUCT ====
		public async Task<CreateProductViewModel> Create(CreateProductViewModel model)
		{
			if (await context.Products.AnyAsync(x => x.Name == model.Name))
				return null;

			var entity = new Product(model.Name, model.Price)
			{
				ImageUrl = model.ImageUrl,
				Category = model.Category,
				Description = model.Description
			};

			await context.Products.AddAsync(entity);
			await context.SaveChangesAsync();

			// Галерия (ако има)
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

		// ==== MY CART (DB) ====
		public async Task<MyCartViewModel> GetMyProducts(User user)
		{
			var myCartProducts = await context.CartProduct
				.Include(x => x.Product)
				.Include(x => x.Cart)
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

		// ==== DELETE PRODUCT ====
		public async Task<bool> Delete(string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return false;

			var entity = await context.Products.FindAsync(id);
			if (entity == null) return false;

			context.Products.Remove(entity);
			await context.SaveChangesAsync();
			return true;
		}

		// ==== CATEGORIES ====
		public async Task<List<string>> GetAllCategoriesAsync(CancellationToken ct = default)
		{
			return await context.Products
				.AsNoTracking()
				.Select(p => p.Category)
				.Where(c => !string.IsNullOrWhiteSpace(c))
				.Distinct()
				.OrderBy(c => c)
				.ToListAsync(ct);
		}

		public async Task<List<string>> GetCategories()
		{
			return await context.Products
				.AsNoTracking()
				.Select(p => p.Category)
				.Where(c => !string.IsNullOrWhiteSpace(c))
				.Distinct()
				.OrderBy(c => c)
				.ToListAsync();
		}

		// ==== QUERY WITH FILTERS + PAGING ====
		public async Task<PagedResult<ProductViewModel>> QueryAsync(ProductQueryOptions options, CancellationToken ct = default)
		{
			IQueryable<Product> q = context.Products.AsNoTracking();

			// normalize min/max
			if (options.MinPrice.HasValue && options.MaxPrice.HasValue &&
				options.MaxPrice.Value < options.MinPrice.Value)
			{
				(options.MinPrice, options.MaxPrice) = (options.MaxPrice, options.MinPrice);
			}

			// search
			if (!string.IsNullOrWhiteSpace(options.Search))
			{
				var s = options.Search.Trim();
				q = q.Where(p =>
					EF.Functions.ILike(p.Name, $"%{s}%") ||
					(p.Description != null && EF.Functions.ILike(p.Description, $"%{s}%")) ||
					(p.Category != null && EF.Functions.ILike(p.Category, $"%{s}%"))
				);
			}

			// categories
			if (options.Categories is { Count: > 0 })
			{
				var cats = options.Categories.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
				if (cats.Count > 0)
					q = q.Where(p => cats.Contains(p.Category));
			}

			// price range
			if (options.MinPrice.HasValue) q = q.Where(p => p.Price >= options.MinPrice.Value);
			if (options.MaxPrice.HasValue) q = q.Where(p => p.Price <= options.MaxPrice.Value);

			var total = await q.CountAsync(ct);

			// sorting
			IOrderedQueryable<Product> ordered = options.SortBy switch
			{
				ProductSortBy.Price => options.Desc ? q.OrderByDescending(p => p.Price) : q.OrderBy(p => p.Price),
				ProductSortBy.Newest => options.Desc ? q.OrderByDescending(p => p.Id) : q.OrderBy(p => p.Id), // ако нямаш CreatedAt
				ProductSortBy.Name => options.Desc ? q.OrderByDescending(p => p.Name) : q.OrderBy(p => p.Name),
				_ /* Relevance */    => options.Desc ? q.OrderBy(p => p.Name).ThenByDescending(p => p.Id)
													 : q.OrderBy(p => p.Name)
			};

			var pageSize = Math.Clamp(options.PageSize, 1, 200);
			var page = Math.Max(1, options.Page);
			var skip = (page - 1) * pageSize;

			var items = await ordered
				.Skip(skip).Take(pageSize)
				.Select(p => new ProductViewModel
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					ImageUrl = p.ImageUrl,
					Category = p.Category,
					Description = p.Description
				})
				.ToListAsync(ct);

			return new PagedResult<ProductViewModel>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};
		}

		// ==== EDIT ====
		public async Task<EditProductViewModel?> GetForEditAsync(string id, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(id)) return null;

			return await context.Products
				.AsNoTracking()
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
		}

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

		// ==== DETAILS (simple – за назад съвместимост) ====
		public async Task<ProductDetailsViewModel?> GetDetailsAsync(string id, CancellationToken ct)
		{
			var p = await context.Products
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id, ct);

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

		// ==== DETAILS (full: reviews + similar + gallery, с email/username) ====
		public async Task<ProductDetailsViewModel?> GetDetailsAsync(string id, string? currentUserId, bool isAdmin, CancellationToken ct)
		{
			var p = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
			if (p == null) return null;

			// images
			var extras = await context.ProductImages
				.Where(i => i.ProductId == id)
				.OrderBy(i => i.SortOrder).ThenBy(i => i.Id)
				.Select(i => new ProductDetailsViewModel.ImageItem { Id = i.Id, Url = i.Url })
				.ToListAsync(ct);

			var images = new List<ProductDetailsViewModel.ImageItem>();
			if (!string.IsNullOrWhiteSpace(p.ImageUrl))
				images.Add(new ProductDetailsViewModel.ImageItem { Id = null, Url = p.ImageUrl! });
			images.AddRange(extras);

			// reviews + Email/UserName
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
					   })
					   .ToListAsync(ct);

			var avg = reviewRows.Count == 0 ? 0 : reviewRows.Average(x => x.Rating);

			// similar
			var similar = await context.Products.AsNoTracking()
				.Where(x => x.Category == p.Category && x.Id != p.Id)
				.OrderByDescending(x => x.Id) // ако нямаш CreatedAt
				.Take(8)
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
					UserName = x.Display, // показваме имейл/username
					Rating = x.Rating,
					Content = x.Content,
					CreatedAt = x.CreatedAt,
					CanDelete = isAdmin || (currentUserId != null && x.UserId == currentUserId)
				}).ToList(),
				Similar = similar,
				Images = images
			};
		}

		// ==== REVIEWS ====
		public async Task<bool> AddReviewAsync(AddReviewInput input, string userId, CancellationToken ct)
		{
			// 1) временно условие: да има в количката този продукт (докато добавим реални поръчки)
			var hasInCart = await context.CartProduct
				.AnyAsync(cp => cp.ProductId == input.ProductId && cp.Cart.UserId == userId, ct);
			if (!hasInCart) return false;

			// 2) 1 ревю / потребител / продукт
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

		// ==== IMAGES (gallery) ====
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
