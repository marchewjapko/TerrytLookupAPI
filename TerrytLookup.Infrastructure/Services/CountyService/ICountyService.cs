using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Infrastructure.Services.CountyService;

public interface ICountyService
{
    public Task AddRange(IEnumerable<CreateCountyDto> counties);

    public IEnumerable<CountyDto> BrowseAllAsync(string? name = null, int? voivodeshipId = null);

    public Task<CountyDto> GetByIdAsync(int voivodeshipId, int countyId);

    Task<bool> ExistAnyAsync();
}