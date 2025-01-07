using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Infrastructure.Services.TownService;

public class TownService(ITownRepository townRepository) : ITownService
{
    public Task AddRange(IEnumerable<SimcDto> towns)
    {
        var entities = towns.Select(x => x.ToDomain());

        return townRepository.AddRangeAsync(entities);
    }

    public IAsyncEnumerable<TownDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null,
        int? countyId = null)
    {
        var towns = townRepository.BrowseAllAsync(name, voivodeshipId, countyId);

        return towns.Select(x => x.ToDto());
    }

    public async Task<TownDto> GetByIdAsync(int id)
    {
        var town = await townRepository.GetByIdAsync(id);

        if (town is null) throw new TownNotFoundException(id);

        return town.ToDto();
    }

    public Task<bool> ExistAnyAsync()
    {
        return townRepository.ExistAnyAsync();
    }
}