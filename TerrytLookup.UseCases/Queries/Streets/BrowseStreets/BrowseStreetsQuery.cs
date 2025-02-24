using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Streets.BrowseStreets;

public record BrowseStreetsQuery(string? name = null, int? townId = null) : IStreamRequest<StreetDto>;