using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PaladinHub.Models;

namespace PaladinHub.Services
{
	public interface ICartStore
	{
		Task AddOrUpdateAsync(string userId, Guid productId, int quantity, CancellationToken ct);
		Task<IReadOnlyList<CartLine>> GetAsync(string userId, CancellationToken ct);
		Task ClearAsync(string userId, CancellationToken ct);
	}
}
