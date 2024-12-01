using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;

namespace TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;

public sealed class TerrytReader : ITerrytReader
{
    private static readonly CsvConfiguration CsvConfig = new(CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
        Mode = CsvMode.NoEscape
    };

    /// <summary>
    ///     Asynchronously reads records of type <typeparamref name="T" /> from a CSV source.
    /// </summary>
    /// <typeparam name="T">The type of the records to read.</typeparam>
    /// <returns>
    ///     A  <c> <![CDATA[ Task<List<T>> ]]> </c> that represents the asynchronous operation.
    ///     The task result contains a list of records of type <typeparamref name="T" />.
    /// </returns>
    /// <remarks>
    ///     This method retrieves records asynchronously and disposes of the resources
    ///     once the operation is complete.
    /// </remarks>
    public Task<HashSet<T>> ReadAsync<T>(IFormFile terrytCsvFile)
    {
        var streamReader = new StreamReader(terrytCsvFile.OpenReadStream());
        var csvReader = new CsvReader(streamReader, CsvConfig);

        var records = csvReader.GetRecordsAsync<T>();

        var result = records.ToHashSetAsync()
            .AsTask();

        result.ContinueWith(_ => {
            streamReader.Dispose();
            csvReader.Dispose();
        });

        return result;
    }
}