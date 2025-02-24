using MediatR;

namespace TerrytLookup.UseCases.Queries.Streets.ExistAnyStreet;

public record ExistAnyStreetQuery : IRequest<bool>;