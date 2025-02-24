using MediatR;
using Microsoft.Extensions.Options;
using TerrytLookup.Core.Options;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Streets.BrowseStreets;

public class BrowseStreetsQueryHandler(IStreetRepository repository, IOptions<RepositoryOptions> options)
    : IStreamRequestHandler<BrowseStreetsQuery, StreetDto>
{
    public IAsyncEnumerable<StreetDto> Handle(BrowseStreetsQuery request, CancellationToken cancellationToken)
    {
        var specification = new StreetGetByFilterSpecification(
            options.Value.StreetPageSize,
            request.name,
            request.townId);

        var streets = repository.BrowseAllAsync(specification);

        return streets.Select(x => x.ToDto());
    }
}