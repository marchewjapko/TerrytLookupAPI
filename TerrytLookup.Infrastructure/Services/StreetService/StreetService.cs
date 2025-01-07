using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Infrastructure.Services.StreetService;

public class StreetService(IStreetRepository streetRepository) : IStreetService
{
    public Task AddRange(IEnumerable<UlicDto> streets)
    {
        var entities = streets.Select(x => x.ToDomain());

        return streetRepository.AddRangeAsync(entities);
    }

    public async Task<StreetDto> GetByIdAsync(int townId, int nameId)
    {
        var street = await streetRepository.GetByIdAsync(townId, nameId);

        if (street is null) throw new StreetNotFoundException(townId, nameId);

        return street.ToDto();
    }

    public Task<bool> ExistAnyAsync()
    {
        return streetRepository.ExistAnyAsync();
    }

    public IAsyncEnumerable<StreetDto> BrowseAllAsync(string? name = null, int? townId = null)
    {
        var streets = streetRepository.BrowseAllAsync(name, townId);

        return streets.Select(x => x.ToDto());
    }
}