//using Microsoft.EntityFrameworkCore;
//using PaladinHub.Data;
//using PaladinHub.Data.Entities;
//using PaladinHub.Data.Models;
//using PaladinHub.Models;
//using PaladinHub.Models.Products;
//using PaladinHub.Services.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//public class ProductServiceTests
//{
//	private AppDbContext CreateDbContext()
//	{
//		var options = new DbContextOptionsBuilder<AppDbContext>()
//			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//			.Options;
//		return new AppDbContext(options);
//	}

//	private async Task SeedDatabase(AppDbContext context)
//	{
//		var product1 = new Product("Product A", 10.0m) { Id = "prod-A", Category = "Category 1" };
//		var product2 = new Product("Product B", 200.0m) { Id = "prod-B", Category = "Category 2" };
//		var product3 = new Product("Product C", 150.0m) { Id = "prod-C", Category = "Category 1" };

//		await context.Products.AddRangeAsync(product1, product2, product3);

//		var image1 = new ProductImage { Id = 1, ProductId = product1.Id, Url = "url_A1", SortOrder = 1 };
//		var image2 = new ProductImage { Id = 2, ProductId = product1.Id, Url = "url_A2", SortOrder = 2 };
//		await context.ProductImages.AddRangeAsync(image1, image2);

//		product1.ThumbnailImageId = image1.Id;

//		// Създаване на ревюта с правилни типове
//		var review1 = new ProductReview { Id = 1, ProductId = product1.Id, UserId = "user-1", Rating = 5, CreatedAt = DateTime.UtcNow };
//		var review2 = new ProductReview { Id = 2, ProductId = product1.Id, UserId = "user-2", Rating = 4, CreatedAt = DateTime.UtcNow.AddMinutes(1) };
//		await context.ProductReviews.AddRangeAsync(review1, review2);

//		var user1 = new User { Id = "user-1", UserName = "testuser1" };
//		var user2 = new User { Id = "user-2", UserName = "testuser2" };
//		await context.Users.AddRangeAsync(user1, user2);

//		var cart = new Cart { Id = Guid.NewGuid(), UserId = user1.Id };
//		await context.Carts.AddAsync(cart);
//		user1.CartId = cart.Id;

//		var cartProduct = new CartProduct { CartId = cart.Id, ProductId = product1.Id, Quantity = 1 };
//		await context.CartProduct.AddAsync(cartProduct);

//		await context.SaveChangesAsync();
//	}

//	[Fact]
//	public async Task GetAll_ShouldReturnAllProductsWithThumbnails()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);

//		// Act
//		var result = await service.GetAll();

//		// Assert
//		Assert.Equal(3, result.Count);
//		Assert.Equal("Product A", result.First(p => p.Id == "prod-A").Name);
//		Assert.Equal("url_A1", result.First(p => p.Id == "prod-A").ImageUrl);
//	}

//	[Fact]
//	public async Task Create_ShouldAddNewProduct()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		var service = new ProductService(context);
//		var model = new CreateProductViewModel
//		{
//			Name = "New Product",
//			Price = 50.0m,
//			Images = new List<ProductImageInputModel>
//			{
//				new ProductImageInputModel { Url = "new_url_1", SortOrder = 1 }
//			},
//			ThumbnailIndex = 0
//		};

//		// Act
//		var result = await service.Create(model);

//		// Assert
//		Assert.NotNull(result);
//		Assert.Single(await context.Products.ToListAsync());
//		var createdProduct = await context.Products.FirstAsync();
//		Assert.Equal("New Product", createdProduct.Name);
//		Assert.NotNull(createdProduct.ThumbnailImageId);
//	}

//	[Fact]
//	public async Task GetMyProducts_ShouldReturnProductsInUserCart()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);
//		var user = await context.Users.FirstAsync(u => u.Id == "user-1");

//		// Act
//		var result = await service.GetMyProducts(user);

//		// Assert
//		Assert.NotNull(result);
//		Assert.Single(result.MyProducts);
//		Assert.Equal("prod-A", result.MyProducts.First().Id);
//		Assert.Equal(10.0m, result.TotalPrice);
//	}

//	[Fact]
//	public async Task Delete_ShouldRemoveProduct()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);

//		// Act
//		var result = await service.Delete("prod-B");

//		// Assert
//		Assert.True(result);
//		Assert.Equal(2, await context.Products.CountAsync());
//		Assert.False(await context.Products.AnyAsync(p => p.Id == "prod-B"));
//	}

//	[Fact]
//	public async Task GetAllCategories_ShouldReturnDistinctCategories()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);

//		// Act
//		var result = await service.GetAllCategoriesAsync();

//		// Assert
//		Assert.Equal(2, result.Count);
//		Assert.Contains("Category 1", result);
//		Assert.Contains("Category 2", result);
//	}

//	[Fact]
//	public async Task GetDetailsAsync_ShouldReturnDetailsWithReviewsAndSimilarProducts()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);

//		// Act
//		var result = await service.GetDetailsAsync("prod-A", "user-1", isAdmin: false, CancellationToken.None);

//		// Assert
//		Assert.NotNull(result);
//		Assert.Equal("Product A", result.Name);
//		Assert.Equal(4.5m, result.AverageRating);
//		Assert.Equal(2, result.ReviewsCount);
//		Assert.Equal(2, result.Reviews.Count);
//		Assert.Single(result.Similar);
//		Assert.Equal("Product C", result.Similar.First().Name);
//		Assert.Equal(2, result.Images.Count);
//	}

//	[Fact]
//	public async Task AddReviewAsync_ShouldAddReview_WhenUserHasProductInCartAndHasNotReviewed()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);
//		var input = new AddReviewInput { ProductId = "prod-A", Rating = 5, Content = "Great product!" };
//		var userId = "user-1";

//		// Act
//		var result = await service.AddReviewAsync(input, userId, CancellationToken.None);

//		// Assert
//		Assert.True(result);
//		Assert.Equal(3, await context.ProductReviews.CountAsync());
//	}

//	[Fact]
//	public async Task DeleteReviewAsync_ShouldRemoveReview_WhenUserIsAuthor()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);

//		// Act
//		var result = await service.DeleteReviewAsync(1, "user-1", isAdmin: false, CancellationToken.None);

//		// Assert
//		Assert.True(result);
//		Assert.Equal(1, await context.ProductReviews.CountAsync());
//	}

//	[Fact]
//	public async Task AddImageAsync_ShouldAddImageToProduct()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);
//		var productId = "prod-A";
//		var newImageUrl = "new_image_url";

//		// Act
//		var result = await service.AddImageAsync(productId, newImageUrl, 3, CancellationToken.None);

//		// Assert
//		Assert.True(result);
//		Assert.Equal(3, await context.ProductImages.CountAsync(i => i.ProductId == productId));
//		Assert.NotNull(await context.ProductImages.FirstOrDefaultAsync(i => i.Url == newImageUrl));
//	}

//	[Fact]
//	public async Task RemoveImageAsync_ShouldRemoveImageFromProduct()
//	{
//		// Arrange
//		using var context = CreateDbContext();
//		await SeedDatabase(context);
//		var service = new ProductService(context);
//		var imageId = 1;

//		// Act
//		var result = await service.RemoveImageAsync(imageId, CancellationToken.None);

//		// Assert
//		Assert.True(result);
//		Assert.False(await context.ProductImages.AnyAsync(i => i.Id == imageId));
//	}
//}