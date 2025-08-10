using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PaladinHub.Data.Entities;
using PaladinHub.Models;
using PaladinHub.Services;

namespace PaladinHub.Services.Carts
{
	public sealed class CartSessionService : ICartSessionService
	{
		private readonly ICartService cartService;
		private readonly ICartStore cartStore;

		public CartSessionService(ICartService cartService, ICartStore cartStore)
		{
			this.cartService = cartService;
			this.cartStore = cartStore;
		}

		public async Task<bool> AddProduct(string productId, string userId, CancellationToken ct)
		{
			bool ok = await cartService.AddProduct(productId, userId); // EF още е string GUID
			if (ok && Guid.TryParse(productId, out var pid))
			{
				var lines = await cartStore.GetAsync(userId, ct);
				var existing = lines.FirstOrDefault(x => x.ProductId == pid);
				int qty = existing == null ? 1 : existing.Quantity + 1;
				await cartStore.AddOrUpdateAsync(userId, pid, qty, ct);
			}
			return ok;
		}

		public async Task<bool> IncreaseProduct(string productId, string userId, CancellationToken ct)
		{
			bool ok = await cartService.IncreaseProduct(productId, userId);
			if (ok && Guid.TryParse(productId, out var pid))
			{
				var lines = await cartStore.GetAsync(userId, ct);
				var existing = lines.FirstOrDefault(x => x.ProductId == pid);
				int qty = existing == null ? 1 : existing.Quantity + 1;
				await cartStore.AddOrUpdateAsync(userId, pid, qty, ct);
			}
			return ok;
		}

		public async Task<bool> DecreaseProduct(string productId, string userId, CancellationToken ct)
		{
			bool ok = await cartService.DecreaseProduct(productId, userId);
			if (ok && Guid.TryParse(productId, out var pid))
			{
				var lines = await cartStore.GetAsync(userId, ct);
				var existing = lines.FirstOrDefault(x => x.ProductId == pid);
				if (existing != null)
				{
					int qty = existing.Quantity > 1 ? existing.Quantity - 1 : 0;
					if (qty > 0)
					{
						await cartStore.AddOrUpdateAsync(userId, pid, qty, ct);
					}
					else
					{
						var filtered = lines.Where(x => x.ProductId != pid).ToList();
						await cartStore.ClearAsync(userId, ct);
						foreach (var line in filtered)
							await cartStore.AddOrUpdateAsync(userId, line.ProductId, line.Quantity, ct);
					}
				}
			}
			return ok;
		}

		public async Task<bool> RemoveProduct(string productId, string userId, CancellationToken ct)
		{
			bool ok = await cartService.RemoveProduct(productId, userId);
			if (ok && Guid.TryParse(productId, out var pid))
			{
				var lines = await cartStore.GetAsync(userId, ct);
				var filtered = lines.Where(x => x.ProductId != pid).ToList();
				await cartStore.ClearAsync(userId, ct);
				foreach (var line in filtered)
					await cartStore.AddOrUpdateAsync(userId, line.ProductId, line.Quantity, ct);
			}
			return ok;
		}

		public async Task ArchiveAndClear(User user, CancellationToken ct)
		{
			await cartService.ArchiveCart(user);
			await cartStore.ClearAsync(user.Id, ct);
		}

		public async Task CleanAndClear(User user, CancellationToken ct)
		{
			await cartService.CleanCart(user);
			await cartStore.ClearAsync(user.Id, ct);
		}

		public async Task SyncRedisToPersistent(User user, CancellationToken ct)
		{
			// 1) Вземаме какво има в Redis
			var lines = await cartStore.GetAsync(user.Id, ct);

			// 2) Изчистваме количката в БД
			await cartService.CleanCart(user);

			// 3) Пълним БД според Redis данните
			foreach (var line in lines)
			{
				var pidStr = line.ProductId.ToString(); // в БД Product.Id е string (GUID)
				for (int i = 0; i < line.Quantity; i++)
				{
					await cartService.AddProduct(pidStr, user.Id);
				}
			}
		}

		public async Task<int> GetCount(string userId, CancellationToken ct)
		{
			var lines = await cartStore.GetAsync(userId, ct);
			return lines?.Sum(x => x.Quantity) ?? 0;
		}

	}
}
