using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Infrastructure.Services.CountyService;

public class CountyService(ICountyRepository countyRepository) : ICountyService
{
    public Task AddRange(IEnumerable<TercDto> counties)
    {
        var entities = counties.Select(x => x.ToDomainCounty());

        return countyRepository.AddRangeAsync(entities);
    }

    public IAsyncEnumerable<CountyDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null)
    {
        var towns = countyRepository.BrowseAllAsync(name, voivodeshipId);

        return towns.Select(x => x.ToDto());
    }

    public async Task<CountyDto> GetByIdAsync(int voivodeshipId, int countyId)
    {
        var town = await countyRepository.GetByIdAsync(voivodeshipId, countyId);

        if (town is null) throw new CountyNotFoundException(voivodeshipId, countyId);

        return town.ToDto();
    }

    public Task<bool> ExistAnyAsync()
    {
        return countyRepository.ExistAnyAsync();
    }
}