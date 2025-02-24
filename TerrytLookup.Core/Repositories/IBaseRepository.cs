using Ardalis.Specification;

namespace TerrytLookup.Core.Repositories;

public interface IBaseRepository<T>
{
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> BrowseAllAsync(ISpecification<T> specification);

    Task<bool> ExistAnyAsync(CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
}