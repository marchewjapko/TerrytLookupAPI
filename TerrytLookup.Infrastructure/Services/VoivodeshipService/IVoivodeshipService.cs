using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Services.VoivodeshipService;

public interface IVoivodeshipService
{
    public Task AddRange(IEnumerable<TercDto> voivodeships);

    public IAsyncEnumerable<VoivodeshipDto> BrowseAllAsync();

    public Task<VoivodeshipDto> GetByIdAsync(int id);

    Task<bool> ExistAnyAsync();
}