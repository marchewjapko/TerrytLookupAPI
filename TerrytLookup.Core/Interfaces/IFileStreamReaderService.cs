namespace TerrytLookup.Core.Interfaces;

public interface IFileStreamReaderService<T>
{
    Task<IList<T>> ReadCsvFromStream(Stream stream, CancellationToken cancellationToken = default);
}