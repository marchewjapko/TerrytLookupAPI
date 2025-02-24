using MediatR;
using TerrytLookup.Core.Repositories;

namespace TerrytLookup.UseCases.Queries.Voivodeships.ExistAnyVoivodeship;

public class ExistAnyVoivodeshipQueryHandler(IVoivodeshipRepository repository)
    : IRequestHandler<ExistAnyVoivodeshipQuery, bool>
{
    public Task<bool> Handle(ExistAnyVoivodeshipQuery request, CancellationToken cancellationToken)
    {
        return repository.ExistAnyAsync(cancellationToken);
    }
}