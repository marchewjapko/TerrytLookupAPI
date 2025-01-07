using TerrytLookup.Core.Domain;

namespace TerrytLookup.Core.Repositories;

public interface ITownRepository
{
    Task AddRangeAsync(IEnumerable<Town> towns);

    IAsyncEnumerable<Town> BrowseAllAsync(string? name = null, int? voivodeshipId = null, int? countyId = null);

    Task<Town?> GetByIdAsync(int id);

    Task<bool> ExistAnyAsync();
}