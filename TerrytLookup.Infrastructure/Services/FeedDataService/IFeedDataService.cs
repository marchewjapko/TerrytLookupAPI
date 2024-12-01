using Microsoft.AspNetCore.Http;

namespace TerrytLookup.Infrastructure.Services.FeedDataService;

public interface IFeedDataService
{
    Task FeedTerrytDataAsync(IFormFile tercCsvFile, IFormFile simcCsvFile, IFormFile ulicCsvFile);
}