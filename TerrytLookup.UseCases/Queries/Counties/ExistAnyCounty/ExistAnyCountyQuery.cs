using MediatR;

namespace TerrytLookup.UseCases.Queries.Counties.ExistAnyCounty;

public record ExistAnyCountyQuery : IRequest<bool>;