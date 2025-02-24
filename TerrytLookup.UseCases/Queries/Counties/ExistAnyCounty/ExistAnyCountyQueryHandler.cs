using MediatR;
using TerrytLookup.Core.Repositories;

namespace TerrytLookup.UseCases.Queries.Counties.ExistAnyCounty;

public class ExistAnyCountyQueryHandler(ICountyRepository repository) : IRequestHandler<ExistAnyCountyQuery, bool>
{
    public Task<bool> Handle(ExistAnyCountyQuery request, CancellationToken cancellationToken)
    {
        return repository.ExistAnyAsync(cancellationToken);
    }
}