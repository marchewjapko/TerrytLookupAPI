using Microsoft.AspNetCore.Http;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.CountyService;
using TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;
using TerrytLookup.Infrastructure.Services.StreetService;
using TerrytLookup.Infrastructure.Services.TownService;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;

namespace TerrytLookup.Infrastructure.Services.FeedDataService;

public class FeedDataService(
    IVoivodeshipService voivodeshipService,
    ICountyService countyService,
    ITownService townService,
    IStreetService streetService,
    ITerrytReader terrytReader) : IFeedDataService
{
    /// <summary>
    ///     Asynchronously processes and feeds data from the provided CSV files into the appropriate data structures.
    /// </summary>
    /// <param name="tercCsvFile">The CSV file containing TERC data, which includes information about voivodeships.</param>
    /// <param name="simcCsvFile">The CSV file containing SIMC data, which includes information about towns.</param>
    /// <param name="ulicCsvFile">The CSV file containing ULIC data, which includes information about streets.</param>
    /// <exception cref="TerrytParsingException">Thrown when an error occurs during the parsing of the CSV files.</exception>
    /// <remarks>
    ///     This method reads data from the provided CSV files concurrently, maps the data into appropriate DTOs,
    ///     and organizes the data into voivodeships, towns, and streets. It also assigns streets to towns and towns to
    ///     voivodeships producing a complete set of data.
    /// </remarks>
    public async Task FeedTerrytDataAsync(IFormFile tercCsvFile, IFormFile simcCsvFile, IFormFile ulicCsvFile)
    {
        try
        {
            var tercTask = terrytReader.ReadAsync<TercDto>(tercCsvFile);
            var simcTask = terrytReader.ReadAsync<SimcDto>(simcCsvFile);
            var ulicTask = terrytReader.ReadAsync<UlicDto>(ulicCsvFile);

            await Task.WhenAll(tercTask, simcTask, ulicTask);

            var terc = await tercTask;
            var simc = await simcTask;
            var ulic = await ulicTask;

            await voivodeshipService.AddRange(terc.Where(x => x.IsVoivodeship()));
            await countyService.AddRange(terc.Where(x => x.IsCounty()));
            await townService.AddRange(simc);
            await streetService.AddRange(ulic);
        }
        catch (Exception ex)
        {
            throw new TerrytParsingException(ex);
        }
    }
}