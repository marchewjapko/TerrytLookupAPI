using MediatR;
using TerrytLookup.Core.Repositories;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Commands.AddCounties;

public class AddCountiesCommandHandler(ICountyRepository countyRepository) : IRequestHandler<AddCountiesCommand>
{
    public Task Handle(AddCountiesCommand request, CancellationToken cancellationToken)
    {
        var entities = request.Counties.Select(x => x.ToDomainCounty());

        return countyRepository.AddRangeAsync(entities, cancellationToken);
    }
}