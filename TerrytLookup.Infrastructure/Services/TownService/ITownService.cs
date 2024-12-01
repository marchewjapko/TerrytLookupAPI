using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Infrastructure.Services.TownService;

public interface ITownService
{
    public Task AddRange(IEnumerable<CreateTownDto> towns);

    public IEnumerable<TownDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null, int? countyId = null);

    public Task<TownDto> GetByIdAsync(int id);

    Task<bool> ExistAnyAsync();
}