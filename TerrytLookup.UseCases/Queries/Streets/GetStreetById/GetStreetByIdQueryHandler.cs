using MediatR;
using TerrytLookup.Core.Exceptions.CustomExceptions;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Streets.GetStreetById;

public class GetStreetByIdQueryHandler(IStreetRepository repository) : IRequestHandler<GetStreetByIdQuery, StreetDto>
{
    public async Task<StreetDto> Handle(GetStreetByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new StreetGetByIdSpecification(request.townId, request.nameId);

        var result = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        if (result == null)
            throw new StreetNotFoundException(request.townId, request.nameId);

        return result.ToDto();
    }
}