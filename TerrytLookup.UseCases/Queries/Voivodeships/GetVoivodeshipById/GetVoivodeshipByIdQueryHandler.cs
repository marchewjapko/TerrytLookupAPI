using MediatR;
using TerrytLookup.Core.Exceptions.CustomExceptions;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Voivodeships.GetVoivodeshipById;

public class GetVoivodeshipByIdQueryHandler(IVoivodeshipRepository repository)
    : IRequestHandler<GetVoivodeshipByIdQuery, VoivodeshipDto>
{
    public async Task<VoivodeshipDto> Handle(GetVoivodeshipByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new VoivodeshipGetByIdSpecification(request.id);

        var voivodeship = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        if (voivodeship is null)
            throw new VoivodeshipNotFoundException(request.id);

        return voivodeship.ToDto();
    }
}