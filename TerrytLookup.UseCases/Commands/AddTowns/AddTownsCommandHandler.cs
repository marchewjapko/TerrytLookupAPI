using MediatR;
using TerrytLookup.Core.Repositories;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Commands.AddTowns;

public class AddTownsCommandHandler(ITownRepository townRepository) : IRequestHandler<AddTownsCommand>
{
    public Task Handle(AddTownsCommand request, CancellationToken cancellationToken)
    {
        var entities = request.Towns.Select(x => x.ToDomain());

        return townRepository.AddRangeAsync(entities);
    }
}