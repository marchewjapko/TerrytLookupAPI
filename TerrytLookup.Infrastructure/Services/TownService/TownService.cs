using AutoMapper;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Infrastructure.Services.TownService;

public class TownService(ITownRepository townRepository, IMapper mapper) : ITownService
{
    public Task AddRange(IEnumerable<CreateTownDto> towns)
    {
        var entities = mapper.Map<IEnumerable<Town>>(towns)
            .ToList();

        return townRepository.AddRangeAsync(entities);
    }

    public IEnumerable<TownDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null, int? countyId = null)
    {
        var towns = townRepository.BrowseAllAsync(name, voivodeshipId, countyId);

        return mapper.Map<IEnumerable<TownDto>>(towns).OrderBy(x => x.Name);
    }

    public async Task<TownDto> GetByIdAsync(int id)
    {
        var town = await townRepository.GetByIdAsync(id);

        if (town is null)
        {
            throw new TownNotFoundException(id);
        }

        return mapper.Map<TownDto>(town);
    }

    public Task<bool> ExistAnyAsync()
    {
        return townRepository.ExistAnyAsync();
    }
}