using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Infrastructure.Services.VoivodeshipService;

public class VoivodeshipService(IVoivodeshipRepository voivodeshipRepository) : IVoivodeshipService
{
    public Task AddRange(IEnumerable<TercDto> voivodeships)
    {
        var entities = voivodeships.Select(x => x.ToDomainVoivodeship());

        return voivodeshipRepository.AddRangeAsync(entities);
    }

    public IAsyncEnumerable<VoivodeshipDto> BrowseAllAsync()
    {
        var voivodeships = voivodeshipRepository.BrowseAllAsync();

        return voivodeships.Select(x => x.ToDto());
    }

    public async Task<VoivodeshipDto> GetByIdAsync(int id)
    {
        var voivodeship = await voivodeshipRepository.GetByIdAsync(id);

        if (voivodeship is null) throw new VoivodeshipNotFoundException(id);

        return voivodeship.ToDto();
    }

    public Task<bool> ExistAnyAsync()
    {
        return voivodeshipRepository.ExistAnyAsync();
    }
}