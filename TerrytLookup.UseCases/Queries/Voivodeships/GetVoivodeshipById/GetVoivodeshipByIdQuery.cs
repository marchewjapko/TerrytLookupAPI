using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Voivodeships.GetVoivodeshipById;

public record GetVoivodeshipByIdQuery(int id) : IRequest<VoivodeshipDto>;