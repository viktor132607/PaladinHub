using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaladinHub.Services.Carts
{
	public class CartService : ICartService
	{
		private readonly AppDbContext context;
		private readonly UserManager<User> userManager;

		public CartService(AppDbContext context, UserManager<User> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		public async Task<bool> AddProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
				return false;

			User user = await context.User.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				return false;

			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);

			if (myCart == null)
			{
				myCart = new Cart
				{
					UserId = user.Id,
					User = user,
					UpdatedOn = DateTime.UtcNow
				};

				user.Cart = myCart;
				user.CartId = myCart.Id;

				await context.Carts.AddAsync(myCart);

				var newCartProduct = new CartProduct
				{
					CartId = myCart.Id,
					ProductId = product.Id,
					Quantity = 1
				};
				context.CartProduct.Add(newCartProduct);

				await context.SaveChangesAsync();
				return true;
			}

			CartProduct cartProduct = await context.CartProduct
				.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

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
			myCart.UpdatedOn = DateTime.UtcNow;
			await context.SaveChangesAsync();

			return true;
		}

		public async Task ArchiveCart(User user)
		{
			Cart cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (cart != null)
			{
				cart.IsArchived = true;
				cart.OrderDate = DateTime.Now.ToString("dd - MM - yyyy  -->  HH:mm:ss");
				cart.UpdatedOn = DateTime.UtcNow;

				Cart newCart = new Cart
				{
					User = user,
					UserId = user.Id,
					UpdatedOn = DateTime.UtcNow
				};

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

			var cart = await context.Carts.FirstOrDefaultAsync(c => c.Id == user.CartId);
			if (cart != null)
				cart.UpdatedOn = DateTime.UtcNow;

			await context.SaveChangesAsync();
		}

		public async Task<bool> IncreaseProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
				return false;

			User user = await context.User.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				return false;

			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct
					.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null)
				{
					cartProduct.Quantity++;
					myCart.UpdatedOn = DateTime.UtcNow;
				}
				await context.SaveChangesAsync();
			}
			return true;
		}

		public async Task<bool> DecreaseProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
				return false;

			User user = await context.User.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				return false;

			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct
					.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null && cartProduct.Quantity > 1)
				{
					cartProduct.Quantity--;
					myCart.UpdatedOn = DateTime.UtcNow;
				}
				else if (cartProduct != null && cartProduct.Quantity == 1)
				{
					context.CartProduct.Remove(cartProduct);
					myCart.UpdatedOn = DateTime.UtcNow;
				}
				await context.SaveChangesAsync();
			}
			return true;
		}

		public async Task<bool> RemoveProduct(string id, string userId)
		{
			Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
				return false;

			User user = await context.User.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				return false;

			Cart myCart = await context.Carts.FirstOrDefaultAsync(x => x.Id == user.CartId);
			if (myCart != null)
			{
				CartProduct cartProduct = await context.CartProduct
					.FirstOrDefaultAsync(x => x.CartId == myCart.Id && x.ProductId == product.Id);

				if (cartProduct != null)
				{
					context.CartProduct.Remove(cartProduct);
					myCart.UpdatedOn = DateTime.UtcNow;
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
				})
				.OrderByDescending(x => x.OrderDate)
				.ToListAsync();

			foreach (var cart in archive)
			{
				cart.User = await userManager.FindByIdAsync(cart.UserId);
			}
			return archive;
		}

		public async Task<MyCartViewModel?> GetCartById(Guid cartId)
		{
			var myCartProducts = await context.CartProduct
				.Include(x => x.Product)
				.Include(x => x.Cart)
				.Where(x => x.CartId == cartId)
				.ToListAsync();

			if (!myCartProducts.Any())
				return null;

			var myCartViewModel = new MyCartViewModel
			{
				MyProducts = new List<ProductViewModel>(),
				TotalPrice = 0m
			};

			foreach (var cartProduct in myCartProducts)
			{
				if (!myCartViewModel.MyProducts.Any(p => p.Id == cartProduct.ProductId))
				{
					var productViewModel = new ProductViewModel
					{
						Id = cartProduct.ProductId,
						Name = cartProduct.Product?.Name,
						Price = cartProduct.Product?.Price ?? 0m,
						Quantity = cartProduct.Quantity,
						CartId = cartProduct.CartId,
						Cart = cartProduct.Cart
					};

					myCartViewModel.MyProducts.Add(productViewModel);
				}

				myCartViewModel.TotalPrice += (cartProduct.Product?.Price ?? 0m) * cartProduct.Quantity;
			}

			return myCartViewModel;
		}
	}
}
