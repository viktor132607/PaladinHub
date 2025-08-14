using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;
using PaladinHub.Services.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class CartServiceTests
{
	// Помощен метод за създаване на нова in-memory база данни
	private AppDbContext CreateDbContext()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;
		return new AppDbContext(options);
	}

	// Помощен метод за запълване на базата данни с примерни данни
	private async Task SeedDatabase(AppDbContext context)
	{
		var user1 = new User { Id = "user-1", UserName = "testuser1", Email = "user1@test.com" };
		var user2 = new User { Id = "user-2", UserName = "testuser2", Email = "user2@test.com" };
		context.Users.AddRange(user1, user2);

		var product1 = new Product { Id = "prod-1", Name = "Product 1", Price = 10.0m };
		var product2 = new Product { Id = "prod-2", Name = "Product 2", Price = 20.0m };
		context.Products.AddRange(product1, product2);

		var cart1 = new Cart { Id = Guid.NewGuid(), UserId = user1.Id };
		var cart2 = new Cart { Id = Guid.NewGuid(), UserId = user2.Id, IsArchived = true };
		context.Carts.AddRange(cart1, cart2);

		user1.CartId = cart1.Id;
		user2.CartId = cart2.Id;

		var cartProduct = new CartProduct { CartId = cart1.Id, ProductId = product1.Id, Quantity = 2 };
		context.CartProduct.Add(cartProduct);

		await context.SaveChangesAsync();
	}

	[Fact]
	public async Task AddProduct_ShouldAddProductToCart_WhenCartExists()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-2";

		// Act
		var result = await service.AddProduct(productId, userId);

		// Assert
		Assert.True(result);
		var cartProduct = await context.CartProduct.FirstOrDefaultAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		Assert.NotNull(cartProduct);
		Assert.Equal(1, cartProduct.Quantity);
	}

	[Fact]
	public async Task AddProduct_ShouldIncreaseQuantity_WhenProductAlreadyInCart()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-1";

		// Act
		var result = await service.AddProduct(productId, userId);

		// Assert
		Assert.True(result);
		var cartProduct = await context.CartProduct.FirstOrDefaultAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		Assert.NotNull(cartProduct);
		Assert.Equal(3, cartProduct.Quantity);
	}

	[Fact]
	public async Task ArchiveCart_ShouldArchiveCurrentCartAndCreateNewOne()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var user = await context.Users.FirstAsync(u => u.Id == "user-1");

		var oldCartId = user.CartId;

		// Act
		await service.ArchiveCart(user);

		// Assert
		var oldCart = await context.Carts.FindAsync(oldCartId);
		Assert.NotNull(oldCart);
		Assert.True(oldCart.IsArchived);
		Assert.NotNull(oldCart.OrderDate);

		Assert.NotEqual(oldCartId, user.CartId);
		Assert.NotNull(user.CartId);
		var newCart = await context.Carts.FindAsync(user.CartId);
		Assert.NotNull(newCart);
		Assert.False(newCart.IsArchived);
	}

	[Fact]
	public async Task CleanCart_ShouldRemoveAllProductsFromCart()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var user = await context.Users.FirstAsync(u => u.Id == "user-1");

		// Act
		await service.CleanCart(user);

		// Assert
		var cartProductsCount = await context.CartProduct.Where(cp => cp.Cart.UserId == user.Id).CountAsync();
		Assert.Equal(0, cartProductsCount);
	}

	[Fact]
	public async Task IncreaseProduct_ShouldIncreaseQuantity_WhenProductExists()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-1";

		var initialQuantity = await context.CartProduct.Where(cp => cp.Cart.UserId == userId && cp.ProductId == productId).Select(cp => cp.Quantity).FirstAsync();

		// Act
		await service.IncreaseProduct(productId, userId);

		// Assert
		var updatedQuantity = await context.CartProduct.Where(cp => cp.Cart.UserId == userId && cp.ProductId == productId).Select(cp => cp.Quantity).FirstAsync();
		Assert.Equal(initialQuantity + 1, updatedQuantity);
	}

	[Fact]
	public async Task DecreaseProduct_ShouldDecreaseQuantity_WhenQuantityIsMoreThanOne()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-1";

		// Act
		await service.DecreaseProduct(productId, userId);

		// Assert
		var cartProduct = await context.CartProduct.FirstOrDefaultAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		Assert.NotNull(cartProduct);
		Assert.Equal(1, cartProduct.Quantity);
	}

	[Fact]
	public async Task DecreaseProduct_ShouldRemoveProduct_WhenQuantityIsOne()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-1";

		var cartProduct = await context.CartProduct.FirstAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		cartProduct.Quantity = 1;
		await context.SaveChangesAsync();

		// Act
		await service.DecreaseProduct(productId, userId);

		// Assert
		var productInCart = await context.CartProduct.AnyAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		Assert.False(productInCart);
	}

	[Fact]
	public async Task RemoveProduct_ShouldRemoveProductFromCart()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var userId = "user-1";
		var productId = "prod-1";

		// Act
		await service.RemoveProduct(productId, userId);

		// Assert
		var productInCart = await context.CartProduct.AnyAsync(cp => cp.Cart.UserId == userId && cp.ProductId == productId);
		Assert.False(productInCart);
	}

	[Fact]
	public async Task GetArchive_ShouldReturnOnlyArchivedCarts()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		mockUserManager.Setup(um => um.FindByIdAsync("user-2")).ReturnsAsync(new User { Id = "user-2", UserName = "testuser2" });
		var service = new CartService(context, mockUserManager.Object);

		// Act
		var archive = await service.GetArchive();

		// Assert
		Assert.NotNull(archive);
		Assert.Single(archive);
		Assert.True(archive.First().OrderDate != null);
		Assert.Equal("testuser2", archive.First().User.UserName);
	}

	[Fact]
	public async Task GetCartById_ShouldReturnCartViewModel_WhenCartExists()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var mockUserManager = new Mock<UserManager<User>>(
			Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
		var service = new CartService(context, mockUserManager.Object);
		var user = await context.Users.FirstAsync(u => u.Id == "user-1");

		// Act
		var cartViewModel = await service.GetCartById(user.CartId.Value);

		// Assert
		Assert.NotNull(cartViewModel);
		Assert.Single(cartViewModel.MyProducts);
		Assert.Equal(20.0m, cartViewModel.TotalPrice);
	}
}