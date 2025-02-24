using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Towns.BrowseTowns;

public record BrowseTownsQuery(string? name = null, int? voivodeshipId = null, int? countyId = null)
    : IStreamRequest<TownDto>;