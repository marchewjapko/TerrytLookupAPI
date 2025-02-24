using MediatR;
using Microsoft.Extensions.Options;
using TerrytLookup.Core.Options;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Core.Specifications;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Mappers;

namespace TerrytLookup.UseCases.Queries.Voivodeships.BrowseVoivodeships;

public class BrowseVoivodeshipsQueryHandler(IVoivodeshipRepository repository, IOptions<RepositoryOptions> options)
    : IStreamRequestHandler<BrowseVoivodeshipsQuery, VoivodeshipDto>
{
    public IAsyncEnumerable<VoivodeshipDto> Handle(BrowseVoivodeshipsQuery request, CancellationToken cancellationToken)
    {
        var specification = new VoivodeshipGetAllSpecification(options.Value.VoivodeshipPageSize);

        var voivodeships = repository.BrowseAllAsync(specification);

        return voivodeships.Select(x => x.ToDto());
    }
}