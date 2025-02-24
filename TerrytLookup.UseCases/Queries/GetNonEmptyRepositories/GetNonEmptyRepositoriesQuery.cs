using MediatR;

namespace TerrytLookup.UseCases.Queries.GetNonEmptyRepositories;

public record GetNonEmptyRepositoriesQuery : IRequest<IList<string>>;