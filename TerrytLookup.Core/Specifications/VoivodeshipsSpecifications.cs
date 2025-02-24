using Ardalis.Specification;
using TerrytLookup.Core.Domain;

namespace TerrytLookup.Core.Specifications;

public sealed class VoivodeshipGetAllSpecification : Specification<Voivodeship>
{
    public VoivodeshipGetAllSpecification(int pageSize)
    {
        Query
            .AsNoTracking()
            .Take(pageSize)
            .OrderBy(x => x.Name);
    }
}

public sealed class VoivodeshipGetByIdSpecification : Specification<Voivodeship>,
    ISingleResultSpecification<Voivodeship>
{
    public VoivodeshipGetByIdSpecification(int voivodeshipId)
    {
        Query.AsNoTracking();

        Query.Where(x => x.Id == voivodeshipId);
    }
}