using Ardalis.Specification;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;

namespace TerrytLookup.Core.Specifications;

public static class CountyIncludes
{
    public static ISpecificationBuilder<County> AddCountyIncludes(this ISpecificationBuilder<County> specification)
    {
        return specification.Include(x => x.Voivodeship);
    }
}

public sealed class CountyGetByFilterSpecification : Specification<County>
{
    public CountyGetByFilterSpecification(int pageSize, string? name = null, int? voivodeshipId = null)
    {
        Query
            .AddCountyIncludes()
            .AsNoTracking()
            .Take(pageSize)
            .OrderBy(x => x.Name);

        if (name is not null)
        {
            var normalizedName = name.NormalizeName();

            Query.Where(x => x.NormalizedName.Contains(normalizedName));
        }

        if (voivodeshipId is not null)
            Query.Where(x => x.VoivodeshipId == voivodeshipId);
    }
}

public sealed class CountyGetByIdSpecification : Specification<County>
{
    public CountyGetByIdSpecification(int voivodeshipId, int countyId)
    {
        Query
            .AddCountyIncludes()
            .AsNoTracking();

        Query.Where(x => x.VoivodeshipId == voivodeshipId && x.CountyId == countyId);
    }
}