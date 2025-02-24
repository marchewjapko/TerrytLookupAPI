using Grpc.Core;
using MediatR;
using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Queries.Streets.BrowseStreets;
using TerrytLookup.UseCases.Queries.Streets.GetStreetById;
using TerrytLookup.WebAPI.Protos.Mappers;

namespace TerrytLookup.WebAPI.Services;

public class StreetService(IMediator mediator) : StreetApiService.StreetApiServiceBase
{
    public override async Task<BrowseAllStreetsResponse> BrowseAll(
        BrowseAllStreetsRequest request,
        ServerCallContext context)
    {
        var query = new BrowseStreetsQuery(request.Name, request.TownId);

        var result = await mediator
            .CreateStream(query)
            .ToListAsync();

        return new BrowseAllStreetsResponse
        {
            Result =
            {
                result.Select(x => x.ToResponse())
            }
        };
    }

    public override async Task<GetStreetByIdResponse> GetById(GetStreetByIdRequest request, ServerCallContext context)
    {
        var query = new GetStreetByIdQuery(request.TownId, request.NameId);

        var result = await mediator.Send(query);

        return new GetStreetByIdResponse
        {
            Result = result.ToResponse()
        };
    }
}