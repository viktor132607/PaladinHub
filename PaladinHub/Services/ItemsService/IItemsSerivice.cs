using PaladinHub.Data.Entities;

public interface IItemsService
{
	Task<List<Item>> GetAllAsync();
	Task<Item?> GetByIdAsync(int id);
	Task<List<Item>> SearchAsync(string? term);
	Task<(IReadOnlyList<Item> Items, int Total)> GetPagedAsync(int page, int pageSize, string? term = null);
}
