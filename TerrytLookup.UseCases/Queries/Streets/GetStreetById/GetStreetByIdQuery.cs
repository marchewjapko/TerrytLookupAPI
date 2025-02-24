using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Streets.GetStreetById;

public record GetStreetByIdQuery(int townId, int nameId) : IRequest<StreetDto>;