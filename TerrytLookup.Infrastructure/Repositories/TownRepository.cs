using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Infrastructure.Repositories;

public class TownRepository(AppDbContext context) : ITownRepository
{
    public Task AddRangeAsync(IEnumerable<Town> towns)
    {
        return context.BulkInsertAsync(towns);
    }

    public IAsyncEnumerable<Town> BrowseAllAsync(string? name = null, int? voivodeshipId = null, int? countyId = null)
    {
        var query = context.Towns.AsNoTracking().AsQueryable();

        if (name is not null)
            query = query.Where(x =>
                EF.Functions.ILike(EF.Functions.Unaccent(x.Name), EF.Functions.Unaccent($"%{name}%")));

        if (voivodeshipId.HasValue) query = query.Where(x => x.County.Voivodeship.Id == voivodeshipId);

        if (countyId.HasValue) query = query.Where(x => x.County.CountyId == countyId);

        query = query.Where(x => x.ParentTown == null);

        return query.OrderBy(x => x.Name).Take(AppDbContext.PageSize).AsAsyncEnumerable();
    }

    public Task<Town?> GetByIdAsync(int id)
    {
        return context.Towns.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<bool> ExistAnyAsync()
    {
        return context.Towns.AnyAsync();
    }
}