using MediatR;
using TerrytLookup.Core.Repositories;

namespace TerrytLookup.UseCases.Queries.Streets.ExistAnyStreet;

public class ExistAnyStreetQueryHandler(IStreetRepository repository) : IRequestHandler<ExistAnyStreetQuery, bool>
{
    public Task<bool> Handle(ExistAnyStreetQuery request, CancellationToken cancellationToken)
    {
        return repository.ExistAnyAsync(cancellationToken);
    }
}