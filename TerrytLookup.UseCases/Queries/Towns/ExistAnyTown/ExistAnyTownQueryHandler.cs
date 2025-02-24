using MediatR;
using TerrytLookup.Core.Repositories;

namespace TerrytLookup.UseCases.Queries.Towns.ExistAnyTown;

public class ExistAnyTownQueryHandler(ITownRepository repository) : IRequestHandler<ExistAnyTownQuery, bool>
{
    public Task<bool> Handle(ExistAnyTownQuery request, CancellationToken cancellationToken)
    {
        return repository.ExistAnyAsync(cancellationToken);
    }
}