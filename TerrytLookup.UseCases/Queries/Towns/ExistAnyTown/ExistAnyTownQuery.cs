using MediatR;

namespace TerrytLookup.UseCases.Queries.Towns.ExistAnyTown;

public record ExistAnyTownQuery : IRequest<bool>;