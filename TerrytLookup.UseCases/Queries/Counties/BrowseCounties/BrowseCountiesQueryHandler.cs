using MediatR;
using Microsoft.Extensions.Options;
using TerrytLookup.Core.Options;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Counties.BrowseCounties;

public class BrowseCountiesQueryHandler(ICountyRepository repository, IOptions<RepositoryOptions> options)
    : IStreamRequestHandler<BrowseCountiesQuery, CountyDto>
{
    public IAsyncEnumerable<CountyDto> Handle(BrowseCountiesQuery request, CancellationToken cancellationToken)
    {
        var specification = new CountyGetByFilterSpecification(
            options.Value.CountyPageSize,
            request.name,
            request.voivodeshipId);

        var counties = repository.BrowseAllAsync(specification);

        return counties.Select(x => x.ToDto());
    }
}