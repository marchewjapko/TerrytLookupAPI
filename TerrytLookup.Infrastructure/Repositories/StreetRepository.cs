using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Infrastructure.Repositories;

public class StreetRepository(AppDbContext dbContext) : BaseRepository<Street>(dbContext), IStreetRepository
{
    public new IAsyncEnumerable<Street> BrowseAllAsync(ISpecification<Street> specification)
    {
        return dbContext
            .Set<Street>()
            .WithSpecification(specification)
            .GroupBy(
                x => new
                {
                    x.NameId,
                    TownId = x.Town.ParentTownId ?? x.Town.Id
                })
            .Select(x => x.First())
            .AsAsyncEnumerable();
    }
}