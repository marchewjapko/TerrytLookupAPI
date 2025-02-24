using Grpc.Core;
using MediatR;
using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Queries.Towns.BrowseTowns;
using TerrytLookup.UseCases.Queries.Towns.GetTownById;
using TerrytLookup.WebAPI.Protos.Mappers;

namespace TerrytLookup.WebAPI.Services;

public class TownService(IMediator mediator) : TownApiService.TownApiServiceBase
{
    public override async Task<BrowseAllTownsResponse> BrowseAll(
        BrowseAllTownsRequest request,
        ServerCallContext context)
    {
        var query = new BrowseTownsQuery(request.Name, request.VoivodeshipId, request.CountyId);

        var result = await mediator
            .CreateStream(query)
            .ToListAsync();

        return new BrowseAllTownsResponse
        {
            Result =
            {
                result.Select(x => x.ToResponse())
            }
        };
    }

    public override async Task<GetTownsByIdResponse> GetById(GetTownsByIdRequest request, ServerCallContext context)
    {
        var query = new GetTownByIdQuery(request.TownId);

        var result = await mediator.Send(query);

        return new GetTownsByIdResponse
        {
            Result = result.ToResponse()
        };
    }
}