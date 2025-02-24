using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Repositories;

namespace TerrytLookup.Infrastructure.Repositories;

public abstract class BaseRepository<T>(Microsoft.EntityFrameworkCore.DbContext dbContext)
    : IBaseRepository<T> where T : class
{
    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        return dbContext.BulkInsertAsync(entities, b => b.IncludeGraph = true, cancellationToken: cancellationToken);
    }

    public IAsyncEnumerable<T> BrowseAllAsync(ISpecification<T> specification)
    {
        return dbContext
            .Set<T>()
            .WithSpecification(specification)
            .AsAsyncEnumerable();
    }

    public Task<bool> ExistAnyAsync(CancellationToken cancellationToken = default)
    {
        return dbContext
            .Set<T>()
            .AnyAsync(cancellationToken);
    }

    public Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return dbContext
            .Set<T>()
            .WithSpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }
}