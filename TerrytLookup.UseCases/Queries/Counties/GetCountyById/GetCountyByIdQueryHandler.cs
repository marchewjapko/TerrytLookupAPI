using MediatR;
using TerrytLookup.Core.Exceptions.CustomExceptions;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Counties.GetCountyById;

public class GetCountyByIdQueryHandler(ICountyRepository repository) : IRequestHandler<GetCountyByIdQuery, CountyDto>
{
    public async Task<CountyDto> Handle(GetCountyByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new CountyGetByIdSpecification(request.voivodeshipId, request.countyId);

        var result = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        if (result == null)
            throw new CountyNotFoundException(request.voivodeshipId, request.countyId);

        return result.ToDto();
    }
}