using MediatR;
using TerrytLookup.Core.Repositories;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Commands.AddStreets;

public class AddStreetsCommandHandler(IStreetRepository streetRepository) : IRequestHandler<AddStreetsCommand>
{
    public Task Handle(AddStreetsCommand request, CancellationToken cancellationToken)
    {
        var entities = request.Streets.Select(x => x.ToDomain());

        return streetRepository.AddRangeAsync(entities, cancellationToken);
    }
}