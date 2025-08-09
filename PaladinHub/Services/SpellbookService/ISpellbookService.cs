using PaladinHub.Data.Entities;

public interface ISpellbookService
{
	Task<List<Spell>> GetAllAsync();
	Task<Spell?> GetByIdAsync(int id);
	Task<List<Spell>> SearchAsync(string? term);
	Task<(IReadOnlyList<Spell> Items, int Total)> GetPagedAsync(int page, int pageSize, string? term = null);
}
