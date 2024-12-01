using AutoMapper;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Infrastructure.Services.CountyService;

public class CountyService(ICountyRepository countyRepository, IMapper mapper) : ICountyService
{
    public Task AddRange(IEnumerable<CreateCountyDto> counties)
    {
        var entities = mapper.Map<IEnumerable<County>>(counties)
            .ToList();

        return countyRepository.AddRangeAsync(entities);
    }

    public IEnumerable<CountyDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null)
    {
        var towns = countyRepository.BrowseAllAsync(name, voivodeshipId);

        return mapper.Map<IEnumerable<CountyDto>>(towns).OrderBy(x => x.Name);
    }

    public async Task<CountyDto> GetByIdAsync(int voivodeshipId, int countyId)
    {
        var town = await countyRepository.GetByIdAsync(voivodeshipId, countyId);

        if (town is null)
        {
            throw new CountyNotFoundException(voivodeshipId, countyId);
        }

        return mapper.Map<CountyDto>(town);
    }

    public Task<bool> ExistAnyAsync()
    {
        return countyRepository.ExistAnyAsync();
    }
}