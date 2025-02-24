using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Counties.GetCountyById;

public record GetCountyByIdQuery(int voivodeshipId, int countyId) : IRequest<CountyDto>;