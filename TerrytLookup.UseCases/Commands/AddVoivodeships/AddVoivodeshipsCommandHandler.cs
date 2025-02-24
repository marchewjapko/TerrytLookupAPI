using MediatR;
using TerrytLookup.Core.Repositories;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Commands.AddVoivodeships;

public class AddVoivodeshipsCommandHandler(IVoivodeshipRepository voivodeshipRepository)
    : IRequestHandler<AddVoivodeshipsCommand>
{
    public Task Handle(AddVoivodeshipsCommand request, CancellationToken cancellationToken)
    {
        var entities = request.Voivodeships.Select(x => x.ToDomainVoivodeship());

        return voivodeshipRepository.AddRangeAsync(entities, cancellationToken);
    }
}