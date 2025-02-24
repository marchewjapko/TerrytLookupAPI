using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Counties.BrowseCounties;

public record BrowseCountiesQuery(string? name = null, int? voivodeshipId = null) : IStreamRequest<CountyDto>;