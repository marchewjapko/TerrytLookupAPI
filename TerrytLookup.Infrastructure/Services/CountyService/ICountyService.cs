using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Infrastructure.Services.CountyService;

public interface ICountyService
{
    public Task AddRange(IEnumerable<CreateCountyDto> counties);

    public IEnumerable<CountyDto> BrowseAllAsync(string? name, int? voivodeshipId);

    public Task<CountyDto> GetByIdAsync(int voivodeshipId, int countyId);

    Task<bool> ExistAnyAsync();
}