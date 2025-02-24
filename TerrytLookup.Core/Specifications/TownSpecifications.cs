using Ardalis.Specification;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;

namespace TerrytLookup.Core.Specifications;

public static class TownIncludes
{
    public static ISpecificationBuilder<Town> AddTownIncludes(this ISpecificationBuilder<Town> specification)
    {
        return specification
            .Include(x => x.ParentTown)
            .ThenInclude(x => x!.County)
            .ThenInclude(x => x.Voivodeship)
            .Include(x => x.County)
            .ThenInclude(x => x.Voivodeship);
    }
}

public sealed class TownGetByFilterSpecification : Specification<Town>
{
    public TownGetByFilterSpecification(
        int pageSize,
        string? name = null,
        int? voivodeshipId = null,
        int? countyId = null)
    {
        Query
            .AddTownIncludes()
            .AsNoTracking()
            .Take(pageSize)
            .OrderBy(x => x.Name);

        if (name is not null)
        {
            var normalizedName = name.NormalizeName();

            Query.Where(x => x.NormalizedName.Contains(normalizedName));
        }

        if (voivodeshipId.HasValue)
            Query.Where(x => x.County.Voivodeship.Id == voivodeshipId);

        if (countyId.HasValue)
            Query.Where(x => x.County.CountyId == countyId);

        Query.Where(x => x.ParentTown == null);
    }
}

public sealed class TownGetByIdSpecification : Specification<Town>, ISingleResultSpecification<Town>
{
    public TownGetByIdSpecification(int id)
    {
        Query
            .AddTownIncludes()
            .AsNoTracking();

        Query.Where(x => x.Id == id);
    }
}