using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Infrastructure.Repositories;

public class StreetRepository(AppDbContext context) : IStreetRepository
{
    public Task AddRangeAsync(IEnumerable<Street> towns)
    {
        return context.BulkInsertAsync(towns);
    }

    public IAsyncEnumerable<Street> BrowseAllAsync(string? name = null, int? townId = null)
    {
        var query = context.Streets.AsNoTracking().AsQueryable();

        if (name is not null)
            query = query.Where(x =>
                EF.Functions.ILike(EF.Functions.Unaccent(x.Name), EF.Functions.Unaccent($"%{name}%")));

        if (townId.HasValue)
            query = query.Where(x =>
                x.Town.Id == townId || (x.Town.ParentTown != null && x.Town.ParentTown.Id == townId));

        query = query.GroupBy(x => x.Name).Select(x => x.First());

        return query.Take(AppDbContext.PageSize).AsAsyncEnumerable();
    }

    public Task<Street?> GetByIdAsync(int townId, int nameId)
    {
        return context.Streets.FirstOrDefaultAsync(x =>
            (x.TownId == townId || (x.Town.ParentTown != null && x.Town.ParentTown.Id == townId)) &&
            x.NameId == nameId);
    }

    public Task<bool> ExistAnyAsync()
    {
        return context.Streets.AnyAsync();
    }
}