using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
using PaladinHub.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Products
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext context;

		public ProductService(AppDbContext context)
		{
			this.context = context;
		}

		public async Task<ICollection<ProductViewModel>> GetAll()
		{
			ICollection<ProductViewModel> products = await context.Products
				.AsNoTracking()
				.Select(x => new ProductViewModel()
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					ImageUrl = x.ImageUrl,
					Category = x.Category,
					Description = x.Description
				})
				.ToListAsync();

			return products;
		}

		public async Task<CreateProductViewModel> Create(CreateProductViewModel model)
		{
			string productName = model.Name;
			bool isProductExist = await context.Products.AnyAsync(x => x.Name == model.Name);
			if (isProductExist)
			{
				return null;
			}

			decimal productPrice = model.Price;

			Product newProduct = new Product(productName, productPrice)
			{
				ImageUrl = model.ImageUrl,
				Category = model.Category,
				Description = model.Description
			};

			await context.Products.AddAsync(newProduct);
			await context.SaveChangesAsync();
			return model;
		}

		public async Task<MyCartViewModel> GetMyProducts(User user)
		{
			ICollection<CartProduct> myCartProducts = await context.CartProduct
				.Include(x => x.Product)
				.Include(x => x.Cart)
				.Where(x => x.CartId == user.CartId)
				.ToListAsync();

			MyCartViewModel myCartViewModel = new MyCartViewModel();

			foreach (CartProduct cartProduct in myCartProducts)
			{
				if (!myCartViewModel.MyProducts.Any(x => x.Id == cartProduct.ProductId))
				{
					ProductViewModel productViewModel = new ProductViewModel
					{
						Id = cartProduct.ProductId,
						Name = cartProduct.Product.Name,
						Price = cartProduct.Product.Price,
						ImageUrl = cartProduct.Product.ImageUrl,
						Category = cartProduct.Product.Category,
						Description = cartProduct.Product.Description,
						Quantity = cartProduct.Quantity,
						CartId = cartProduct.CartId,
						Cart = cartProduct.Cart
					};
					myCartViewModel.MyProducts.Add(productViewModel);
				}

				myCartViewModel.TotalPrice += (cartProduct.Product.Price * cartProduct.Quantity);
			}

			return myCartViewModel;
		}

		public async Task<bool> Delete(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return false;
			}
			Product product = await context.Products.FindAsync(id);
			if (product == null)
			{
				return false;
			}
			context.Remove(product);
			await context.SaveChangesAsync();
			return true;
		}

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

		public async Task<PagedResult<ProductViewModel>> QueryAsync(ProductQueryOptions options, CancellationToken ct = default)
		{
			IQueryable<Product> q = context.Products.AsNoTracking();

			if (options.MinPrice.HasValue && options.MaxPrice.HasValue &&
				options.MaxPrice.Value < options.MinPrice.Value)
			{
				(options.MinPrice, options.MaxPrice) = (options.MaxPrice, options.MinPrice);
			}

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
				{
					q = q.Where(p => cats.Contains(p.Category));
				}
			}

			if (options.MinPrice.HasValue)
				q = q.Where(p => p.Price >= options.MinPrice.Value);

			if (options.MaxPrice.HasValue)
				q = q.Where(p => p.Price <= options.MaxPrice.Value);

			var total = await q.CountAsync(ct);

			IOrderedQueryable<Product>? ordered = null;
			switch (options.SortBy)
			{
				case ProductSortBy.Price:
					ordered = options.Desc ? q.OrderByDescending(p => p.Price) : q.OrderBy(p => p.Price);
					break;

				case ProductSortBy.Newest:
					ordered = options.Desc ? q.OrderByDescending(p => p.Id) : q.OrderBy(p => p.Id);
					break;

				case ProductSortBy.Name:
					ordered = options.Desc ? q.OrderByDescending(p => p.Name) : q.OrderBy(p => p.Name);
					break;

				case ProductSortBy.Relevance:
				default:
					ordered = q.OrderBy(p => p.Name);
					if (options.Desc) ordered = ordered.ThenByDescending(p => p.Id);
					break;
			}

			var pageSize = Math.Clamp(options.PageSize, 1, 200);
			var page = Math.Max(1, options.Page);
			var skip = (page - 1) * pageSize;

			var items = await ordered!
				.Skip(skip)
				.Take(pageSize)
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

	}
}
