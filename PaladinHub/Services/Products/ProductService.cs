using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Products
{
	public class ProductService : IProductService
	{
		private AppDbContext context;
		public ProductService(AppDbContext context)
		{
			this.context = context;
		}
		public async Task<ICollection<ProductViewModel>> GetAll()
		{
			ICollection<ProductViewModel> products = await
				context.Products.Select(x => new ProductViewModel()
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
				})
				.ToListAsync();

			return products;
		}
		public async Task<CreateProductViewModel> Create(CreateProductViewModel model)
		{
			string productName = model.Name;
			bool IsProductExist = await context.Products.AnyAsync(x => x.Name == model.Name);
			if (IsProductExist)
			{
				return null;
			}
			decimal productPrice = model.Price;
			Product newProduct = new Product(productName, productPrice);
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
						Quantity = cartProduct.Quantity,
						CartId = cartProduct.CartId,
						Cart = cartProduct.Cart
					};
					myCartViewModel.MyProducts.Add(productViewModel);
				}

				myCartViewModel.TotalPrice += (cartProduct.Product.Price * cartProduct.Quantity);
				//myCartViewModel.MyProducts.FirstOrDefault(x => x.Id == cartProduct.ProductId).Quantity++;
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
	}
}