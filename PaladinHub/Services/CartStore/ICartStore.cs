﻿using PaladinHub.Models.Carts;

namespace PaladinHub.Services
{
	public interface ICartStore
	{
		Task AddOrUpdateAsync(string userId, Guid productId, int quantity, CancellationToken ct);
		Task<IReadOnlyList<CartLine>> GetAsync(string userId, CancellationToken ct);
		Task ClearAsync(string userId, CancellationToken ct);
	}
}
