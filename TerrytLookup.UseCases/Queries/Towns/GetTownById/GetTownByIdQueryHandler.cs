using MediatR;
using TerrytLookup.Core.Exceptions.CustomExceptions;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Towns.GetTownById;

public class GetTownByIdQueryHandler(ITownRepository repository) : IRequestHandler<GetTownByIdQuery, TownDto>
{
    public async Task<TownDto> Handle(GetTownByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new TownGetByIdSpecification(request.id);

        var town = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        if (town is null)
            throw new TownNotFoundException(request.id);

        return town.ToDto();
    }
}