using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Towns.GetTownById;

public record GetTownByIdQuery(int id) : IRequest<TownDto>;