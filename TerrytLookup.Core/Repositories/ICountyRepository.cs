using TerrytLookup.Core.Domain;

namespace TerrytLookup.Core.Repositories;

public interface ICountyRepository
{
    Task AddRangeAsync(IList<County> counties);

    IAsyncEnumerable<County> BrowseAllAsync(string? name = null, int? voivodeshipId = null);

    Task<County?> GetByIdAsync(int voivodeshipId, int countyId);

    Task<bool> ExistAnyAsync();
}