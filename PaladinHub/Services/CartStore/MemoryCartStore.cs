using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using PaladinHub.Common;
using PaladinHub.Models.Carts;

namespace PaladinHub.Services
{
	public sealed class MemoryCartStore : ICartStore
	{
		private readonly IDistributedCache cache;

		public MemoryCartStore(IDistributedCache cache) => this.cache = cache;

		private static string Key(string userId) => Constants.Cart.RedisPrefix + userId; // ако нямаш константите – замени с "cart:" и 24 часа

		public async Task AddOrUpdateAsync(string userId, Guid productId, int quantity, CancellationToken ct)
		{
			var key = Key(userId);
			var list = new List<CartLine>(await GetAsync(userId, ct));

			var existing = list.Find(x => x.ProductId == productId);
			if (existing == null)
			{
				list.Add(new CartLine { ProductId = productId, Quantity = Math.Max(1, quantity) });
			}
			else
			{
				existing.Quantity = Math.Max(0, quantity);
				if (existing.Quantity == 0) list.RemoveAll(x => x.ProductId == productId);
			}

			var payload = JsonSerializer.Serialize(list);
			var opts = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(Constants.Cart.TtlHours) // или TimeSpan.FromHours(24)
			};
			await cache.SetStringAsync(key, payload, opts, ct);
		}

		public async Task<IReadOnlyList<CartLine>> GetAsync(string userId, CancellationToken ct)
		{
			var key = Key(userId);
			var value = await cache.GetStringAsync(key, ct);
			if (string.IsNullOrEmpty(value)) return new List<CartLine>();

			var opts = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(Constants.Cart.TtlHours)
			};
			await cache.SetStringAsync(key, value, opts, ct); // refresh TTL

			return JsonSerializer.Deserialize<List<CartLine>>(value!) ?? new List<CartLine>();
		}

		public Task ClearAsync(string userId, CancellationToken ct)
			=> cache.RemoveAsync(Key(userId), ct);
	}
}
