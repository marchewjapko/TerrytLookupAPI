using Ardalis.Specification;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;

namespace TerrytLookup.Core.Specifications;

public static class StreetIncludes
{
    public static ISpecificationBuilder<Street> AddStreetIncludes(this ISpecificationBuilder<Street> specification)
    {
        return specification
            .Include(x => x.Town)
            .ThenInclude(x => x.ParentTown)
            .ThenInclude(x => x!.County)
            .ThenInclude(x => x.Voivodeship)
            .Include(x => x.Town)
            .ThenInclude(x => x.County)
            .ThenInclude(x => x.Voivodeship);
    }
}

public sealed class StreetGetByFilterSpecification : Specification<Street>
{
    public StreetGetByFilterSpecification(int pageSize, string? name = null, int? townId = null)
    {
        Query
            .AddStreetIncludes()
            .AsNoTracking()
            .Take(pageSize)
            .OrderBy(x => x.Name);

        if (name is not null)
        {
            var normalizedName = name.NormalizeName();

            Query.Where(x => x.NormalizedName.Contains(normalizedName));
        }

        if (townId.HasValue)
            Query.Where(x => x.Town.Id == townId || (x.Town.ParentTown != null && x.Town.ParentTown.Id == townId));
    }
}

public sealed class StreetGetByIdSpecification : Specification<Street>
{
    public StreetGetByIdSpecification(int townId, int nameId)
    {
        Query
            .AddStreetIncludes()
            .AsNoTracking();

        Query.Where(x => x.TownId == townId || (x.Town.ParentTown != null && x.Town.ParentTown.Id == townId));

        Query.Where(x => x.NameId == nameId);
    }
}