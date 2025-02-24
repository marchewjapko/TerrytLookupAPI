using MediatR;
using Microsoft.Extensions.Options;
using TerrytLookup.Core.Options;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Towns.BrowseTowns;

public class BrowseTownsQueryHandler(ITownRepository repository, IOptions<RepositoryOptions> options)
    : IStreamRequestHandler<BrowseTownsQuery, TownDto>
{
    public IAsyncEnumerable<TownDto> Handle(BrowseTownsQuery request, CancellationToken cancellationToken)
    {
        var specification = new TownGetByFilterSpecification(
            options.Value.TownPageSize,
            request.name,
            request.voivodeshipId,
            request.countyId);

        var towns = repository.BrowseAllAsync(specification);

        return towns.Select(x => x.ToDto());
    }
}