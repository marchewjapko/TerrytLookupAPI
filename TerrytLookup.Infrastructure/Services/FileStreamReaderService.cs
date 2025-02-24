using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TerrytLookup.Core.Interfaces;

namespace TerrytLookup.Infrastructure.Services;

public class FileStreamReaderService<T> : IFileStreamReaderService<T>
{
    private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
        Mode = CsvMode.NoEscape
    };

    public async Task<IList<T>> ReadCsvFromStream(Stream stream, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _csvConfig);

        var records = await csv
            .GetRecordsAsync<T>(cancellationToken)
            .ToListAsync(cancellationToken);

        await stream.DisposeAsync();

        return records;
    }
}