using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Carts
{
	public class CartService : ICartService
	{
		private AppDbContext context;
		private UserManager<User> userManager;
		public CartService(AppDbContext context, UserManager<User> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}
		public async Task<bool> AddProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return false;
			}

			User user = await context.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
			{
				return false;
			}
			//user.cart.products.add(product);
			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct is null)
				{
					cartProduct = new CartProduct
					{
						CartId = myCart.Id,
						ProductId = product.Id,
						Quantity = 0,
					};
					context.CartProduct.Add(cartProduct);
				}
				cartProduct.Quantity++;
				await context.SaveChangesAsync();
			}
			return true;
		}

		public async Task ArchiveCart(User user)
		{
			Cart cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (cart != null)
			{
				cart.IsArchived = true;
				cart.OrderDate = DateTime.Now.ToString("dd - MM - yyyy  -->  HH:mm:ss");

				Cart newCart = new Cart();
				newCart.User = user;
				newCart.UserId = user.Id;

				user.Cart = newCart;
				user.CartId = newCart.Id;

				await context.Carts.AddAsync(newCart);
				await context.SaveChangesAsync();
			}
		}
		public async Task CleanCart(User user)
		{
			ICollection<CartProduct> myCartProducts = await context.CartProduct
				.Include(x => x.Product)
				.Include(x => x.Cart)
				.Where(x => x.CartId == user.CartId)
				.ToListAsync();

			foreach (CartProduct cartProduct in myCartProducts)
			{
				context.CartProduct.Remove(cartProduct);
			}
			await context.SaveChangesAsync();
		}

		public async Task<bool> IncreaseProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return false;
			}

			User user = await context.User.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
			{
				return false;
			}
			//user.cart.products.add(product);
			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null)
				{
					cartProduct.Quantity++;
				}
				await context.SaveChangesAsync();
			}
			return true;
		}
		public async Task<bool> DecreaseProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return false;
			}

			User user = await context.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
			{
				return false;
			}
			//user.cart.products.add(product);
			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null && cartProduct.Quantity > 1)
				{
					cartProduct.Quantity--;
				}
				else if (cartProduct != null && cartProduct.Quantity == 1)
				{
					context.CartProduct.Remove(cartProduct);
				}
				await context.SaveChangesAsync();
			}
			return true;
		}
		public async Task<bool> RemoveProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return false;
			}

			User user = await context.ApplicationUser.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
			{
				return false;
			}
			//user.cart.products.add(product);
			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null)
				{
					context.CartProduct.Remove(cartProduct);
				}
				await context.SaveChangesAsync();
			}
			return true;
		}

		public async Task<ICollection<CartViewModel>> GetArchive()
		{
			ICollection<CartViewModel> archive = await context.Carts
				.Include(x => x.User)
				.Include(x => x.CartProducts)
				.Include(x => x.Products)
				.Where(x => x.IsArchived)
				.Select(x => new CartViewModel
				{
					Id = x.Id,
					UserId = x.UserId,
					CartProducts = x.CartProducts,
					OrderDate = x.OrderDate,
					Products = x.Products,
				}
				)
				.OrderByDescending(x => x.OrderDate)
				.ToListAsync();
			foreach (var cart in archive)
			{
				cart.User = await userManager.FindByIdAsync(cart.UserId);
			}
			return archive;
		}
		public async Task<MyCartViewModel> GetCartById(string cartId)
		{
			if (string.IsNullOrWhiteSpace(cartId))
			{
				return null;
			}

			ICollection<CartProduct> myCartProducts = await context.CartProduct
				.Include(x => x.Product)
				.Include(x => x.Cart)
				.Where(x => x.CartId == cartId)
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
	}
}