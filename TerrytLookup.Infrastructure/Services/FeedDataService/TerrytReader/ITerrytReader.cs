using Microsoft.AspNetCore.Http;

namespace TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;

public interface ITerrytReader
{
    Task<List<T>> ReadAsync<T>(IFormFile terrytCsvFile);
}