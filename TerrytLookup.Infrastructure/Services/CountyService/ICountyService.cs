using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Services.CountyService;

public interface ICountyService
{
    public Task AddRange(IEnumerable<TercDto> counties);

    public IAsyncEnumerable<CountyDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null);

    public Task<CountyDto> GetByIdAsync(int voivodeshipId, int countyId);

    Task<bool> ExistAnyAsync();
}