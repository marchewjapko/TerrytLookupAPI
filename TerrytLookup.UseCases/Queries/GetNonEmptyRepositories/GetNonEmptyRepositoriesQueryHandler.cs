using MediatR;
using TerrytLookup.Core.Repositories;
using TerrytLookup.UseCases.Queries.Counties.ExistAnyCounty;
using TerrytLookup.UseCases.Queries.Streets.ExistAnyStreet;
using TerrytLookup.UseCases.Queries.Towns.ExistAnyTown;
using TerrytLookup.UseCases.Queries.Voivodeships.ExistAnyVoivodeship;

namespace TerrytLookup.UseCases.Queries.GetNonEmptyRepositories;

public class GetNonEmptyRepositoriesQueryHandler(IMediator mediator)
    : IRequestHandler<GetNonEmptyRepositoriesQuery, IList<string>>
{
    public async Task<IList<string>> Handle(GetNonEmptyRepositoriesQuery request, CancellationToken cancellationToken)
    {
        var result = new List<string>();

        if (await mediator.Send(new ExistAnyVoivodeshipQuery(), cancellationToken))
            result.Add(nameof(IVoivodeshipRepository));

        if (await mediator.Send(new ExistAnyCountyQuery(), cancellationToken))
            result.Add(nameof(ICountyRepository));

        if (await mediator.Send(new ExistAnyTownQuery(), cancellationToken))
            result.Add(nameof(ITownRepository));

        if (await mediator.Send(new ExistAnyStreetQuery(), cancellationToken))
            result.Add(nameof(IStreetRepository));

        return result;
    }
}