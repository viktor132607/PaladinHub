using Moq;
using PaladinHub.Data.Entities;
using PaladinHub.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Services;
using PaladinHub.Services.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class CartSessionServiceTests
{
	private readonly Mock<ICartService> mockCartService;
	private readonly Mock<ICartStore> mockCartStore;
	private readonly CartSessionService cartSessionService;

	public CartSessionServiceTests()
	{
		mockCartService = new Mock<ICartService>();
		mockCartStore = new Mock<ICartStore>();
		cartSessionService = new CartSessionService(mockCartService.Object, mockCartStore.Object);
	}

	[Fact]
	public async Task AddProduct_ShouldAddProductToCartAndRedis_WhenSuccessful()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.AddProduct(productId, userId)).ReturnsAsync(true);
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>());

		// Act
		var result = await cartSessionService.AddProduct(productId, userId, cancellationToken);

		// Assert
		Assert.True(result);
		mockCartService.Verify(x => x.AddProduct(productId, userId), Times.Once);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(userId, Guid.Parse(productId), 1, cancellationToken), Times.Once);
	}

	[Fact]
	public async Task AddProduct_ShouldNotUpdateRedis_WhenCartServiceFails()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.AddProduct(productId, userId)).ReturnsAsync(false);

		// Act
		var result = await cartSessionService.AddProduct(productId, userId, cancellationToken);

		// Assert
		Assert.False(result);
		mockCartStore.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task IncreaseProduct_ShouldIncreaseProductQuantityInCartAndRedis()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.IncreaseProduct(productId, userId)).ReturnsAsync(true);
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>
		{
			new CartLine { ProductId = Guid.Parse(productId), Quantity = 1 }
		});

		// Act
		var result = await cartSessionService.IncreaseProduct(productId, userId, cancellationToken);

		// Assert
		Assert.True(result);
		mockCartService.Verify(x => x.IncreaseProduct(productId, userId), Times.Once);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(userId, Guid.Parse(productId), 2, cancellationToken), Times.Once);
	}

	[Fact]
	public async Task DecreaseProduct_ShouldDecreaseProductQuantityInCartAndRedis_WhenQuantityIsMoreThanOne()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.DecreaseProduct(productId, userId)).ReturnsAsync(true);
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>
		{
			new CartLine { ProductId = Guid.Parse(productId), Quantity = 2 }
		});

		// Act
		var result = await cartSessionService.DecreaseProduct(productId, userId, cancellationToken);

		// Assert
		Assert.True(result);
		mockCartService.Verify(x => x.DecreaseProduct(productId, userId), Times.Once);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(userId, Guid.Parse(productId), 1, cancellationToken), Times.Once);
	}

	[Fact]
	public async Task DecreaseProduct_ShouldRemoveProductFromRedis_WhenQuantityBecomesOneAndIsDecreased()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.DecreaseProduct(productId, userId)).ReturnsAsync(true);
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>
		{
			new CartLine { ProductId = Guid.Parse(productId), Quantity = 1 }
		});

		// Act
		var result = await cartSessionService.DecreaseProduct(productId, userId, cancellationToken);

		// Assert
		Assert.True(result);
		mockCartService.Verify(x => x.DecreaseProduct(productId, userId), Times.Once);
		mockCartStore.Verify(x => x.ClearAsync(userId, cancellationToken), Times.Once);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task RemoveProduct_ShouldRemoveProductFromCartAndRedis()
	{
		// Arrange
		var productId = Guid.NewGuid().ToString();
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;

		mockCartService.Setup(x => x.RemoveProduct(productId, userId)).ReturnsAsync(true);
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>
		{
			new CartLine { ProductId = Guid.Parse(productId), Quantity = 1 }
		});

		// Act
		var result = await cartSessionService.RemoveProduct(productId, userId, cancellationToken);

		// Assert
		Assert.True(result);
		mockCartService.Verify(x => x.RemoveProduct(productId, userId), Times.Once);
		mockCartStore.Verify(x => x.ClearAsync(userId, cancellationToken), Times.Once);
		mockCartStore.Verify(x => x.AddOrUpdateAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task ArchiveAndClear_ShouldArchiveCartAndClearRedis()
	{
		// Arrange
		var user = new User { Id = "test-user" };
		var cancellationToken = CancellationToken.None;

		// Act
		await cartSessionService.ArchiveAndClear(user, cancellationToken);

		// Assert
		mockCartService.Verify(x => x.ArchiveCart(user), Times.Once);
		mockCartStore.Verify(x => x.ClearAsync(user.Id, cancellationToken), Times.Once);
	}

	[Fact]
	public async Task CleanAndClear_ShouldCleanCartAndClearRedis()
	{
		// Arrange
		var user = new User { Id = "test-user" };
		var cancellationToken = CancellationToken.None;

		// Act
		await cartSessionService.CleanAndClear(user, cancellationToken);

		// Assert
		mockCartService.Verify(x => x.CleanCart(user), Times.Once);
		mockCartStore.Verify(x => x.ClearAsync(user.Id, cancellationToken), Times.Once);
	}

	[Fact]
	public async Task GetCount_ShouldReturnCorrectTotalQuantity()
	{
		// Arrange
		var userId = "test-user";
		var cancellationToken = CancellationToken.None;
		mockCartStore.Setup(x => x.GetAsync(userId, cancellationToken)).ReturnsAsync(new List<CartLine>
		{
			new CartLine { ProductId = Guid.NewGuid(), Quantity = 2 },
			new CartLine { ProductId = Guid.NewGuid(), Quantity = 3 }
		});

		// Act
		var count = await cartSessionService.GetCount(userId, cancellationToken);

		// Assert
		Assert.Equal(5, count);
	}

	[Fact]
	public async Task SyncRedisToPersistent_ShouldSyncDataCorrectly()
	{
		// Arrange
		var user = new User { Id = "test-user" };
		var cancellationToken = CancellationToken.None;
		var product1Id = Guid.NewGuid();
		var product2Id = Guid.NewGuid();

		var cartLines = new List<CartLine>
		{
			new CartLine { ProductId = product1Id, Quantity = 2 },
			new CartLine { ProductId = product2Id, Quantity = 1 }
		};

		mockCartStore.Setup(x => x.GetAsync(user.Id, cancellationToken)).ReturnsAsync(cartLines);

		// Act
		await cartSessionService.SyncRedisToPersistent(user, cancellationToken);

		// Assert
		mockCartService.Verify(x => x.CleanCart(user), Times.Once);
		mockCartService.Verify(x => x.AddProduct(product1Id.ToString(), user.Id), Times.Exactly(2));
		mockCartService.Verify(x => x.AddProduct(product2Id.ToString(), user.Id), Times.Once);
	}
}