using Moq;
using PaladinHub.Common;
using PaladinHub.Models.Carts;
using PaladinHub.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class RedisCartStoreTests
{
	private readonly Mock<IDatabase> mockDatabase;
	private readonly Mock<IConnectionMultiplexer> mockConnectionMultiplexer;
	private readonly RedisCartStore redisCartStore;

	public RedisCartStoreTests()
	{
		mockDatabase = new Mock<IDatabase>();
		mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
		mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);
		redisCartStore = new RedisCartStore(mockConnectionMultiplexer.Object);
	}

	[Fact]
	public async Task AddOrUpdateAsync_ShouldAddNewProduct_WhenCartIsEmpty()
	{
		// Arrange
		var userId = "test-user-id";
		var productId = Guid.NewGuid();
		var quantity = 1;
		var redisKey = Constants.Cart.RedisPrefix + userId;

		mockDatabase.Setup(x => x.StringGetAsync(redisKey, It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);

		// Act
		await redisCartStore.AddOrUpdateAsync(userId, productId, quantity, CancellationToken.None);

		// Assert
		var expectedCart = new List<CartLine> { new CartLine { ProductId = productId, Quantity = quantity } };
		var serializedCart = JsonSerializer.Serialize(expectedCart);
		mockDatabase.Verify(x => x.StringSetAsync(redisKey, serializedCart, TimeSpan.FromHours(Constants.Cart.TtlHours), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
	}

	[Fact]
	public async Task AddOrUpdateAsync_ShouldUpdateExistingProduct_WhenProductAlreadyInCart()
	{
		// Arrange
		var userId = "test-user-id";
		var productId = Guid.NewGuid();
		var quantity = 5;
		var redisKey = Constants.Cart.RedisPrefix + userId;

		var existingCart = new List<CartLine> { new CartLine { ProductId = productId, Quantity = 2 } };
		var existingSerializedCart = JsonSerializer.Serialize(existingCart);

		mockDatabase.Setup(x => x.StringGetAsync(redisKey, It.IsAny<CommandFlags>())).ReturnsAsync(existingSerializedCart);

		// Act
		await redisCartStore.AddOrUpdateAsync(userId, productId, quantity, CancellationToken.None);

		// Assert
		var expectedCart = new List<CartLine> { new CartLine { ProductId = productId, Quantity = quantity } };
		var serializedCart = JsonSerializer.Serialize(expectedCart);
		mockDatabase.Verify(x => x.StringSetAsync(redisKey, serializedCart, TimeSpan.FromHours(Constants.Cart.TtlHours), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
	}

	[Fact]
	public async Task GetAsync_ShouldReturnEmptyList_WhenCartIsEmpty()
	{
		// Arrange
		var userId = "test-user-id";
		var redisKey = Constants.Cart.RedisPrefix + userId;

		mockDatabase.Setup(x => x.StringGetAsync(redisKey, It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);

		// Act
		var result = await redisCartStore.GetAsync(userId, CancellationToken.None);

		// Assert
		Assert.Empty(result);
		mockDatabase.Verify(x => x.KeyExpireAsync(redisKey, TimeSpan.FromHours(Constants.Cart.TtlHours), It.IsAny<CommandFlags>()), Times.Never);
	}

	[Fact]
	public async Task GetAsync_ShouldReturnCartLines_WhenCartHasProducts()
	{
		// Arrange
		var userId = "test-user-id";
		var productId1 = Guid.NewGuid();
		var productId2 = Guid.NewGuid();
		var redisKey = Constants.Cart.RedisPrefix + userId;

		var cartLines = new List<CartLine>
		{
			new CartLine { ProductId = productId1, Quantity = 2 },
			new CartLine { ProductId = productId2, Quantity = 1 }
		};
		var serializedCart = JsonSerializer.Serialize(cartLines);

		mockDatabase.Setup(x => x.StringGetAsync(redisKey, It.IsAny<CommandFlags>())).ReturnsAsync(serializedCart);

		// Act
		var result = await redisCartStore.GetAsync(userId, CancellationToken.None);

		// Assert
		Assert.Equal(2, result.Count);
		Assert.Contains(result, x => x.ProductId == productId1 && x.Quantity == 2);
		Assert.Contains(result, x => x.ProductId == productId2 && x.Quantity == 1);
		mockDatabase.Verify(x => x.KeyExpireAsync(redisKey, TimeSpan.FromHours(Constants.Cart.TtlHours), It.IsAny<CommandFlags>()), Times.Once);
	}

	[Fact]
	public async Task ClearAsync_ShouldDeleteTheRedisKey()
	{
		// Arrange
		var userId = "test-user-id";
		var redisKey = Constants.Cart.RedisPrefix + userId;
		mockDatabase.Setup(x => x.KeyDeleteAsync(redisKey, It.IsAny<CommandFlags>())).ReturnsAsync(true);

		// Act
		await redisCartStore.ClearAsync(userId, CancellationToken.None);

		// Assert
		mockDatabase.Verify(x => x.KeyDeleteAsync(redisKey, It.IsAny<CommandFlags>()), Times.Once);
	}
}