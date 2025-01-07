using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Services.StreetService;

public interface IStreetService
{
    public Task AddRange(IEnumerable<UlicDto> streets);

    public Task<StreetDto> GetByIdAsync(int townId, int nameId);

    Task<bool> ExistAnyAsync();

    public IAsyncEnumerable<StreetDto> BrowseAllAsync(string? name = null, int? townId = null);
}