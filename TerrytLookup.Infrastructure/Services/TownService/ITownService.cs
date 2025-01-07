using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Services.TownService;

public interface ITownService
{
    public Task AddRange(IEnumerable<SimcDto> towns);

    public IAsyncEnumerable<TownDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null,
        int? countyId = null);

    public Task<TownDto> GetByIdAsync(int id);

    Task<bool> ExistAnyAsync();
}