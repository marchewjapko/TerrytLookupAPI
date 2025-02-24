using Grpc.Core;
using MediatR;
using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Queries.Counties.BrowseCounties;
using TerrytLookup.UseCases.Queries.Counties.GetCountyById;
using TerrytLookup.WebAPI.Protos.Mappers;

namespace TerrytLookup.WebAPI.Services;

public class CountyService(IMediator mediator) : CountyApiService.CountyApiServiceBase
{
    public override async Task<BrowseAllCountiesResponse> BrowseAll(
        BrowseAllCountiesRequest request,
        ServerCallContext context)
    {
        var query = new BrowseCountiesQuery(request.Name, request.VoivodeshipId);

        var result = await mediator
            .CreateStream(query)
            .ToListAsync();

        return new BrowseAllCountiesResponse
        {
            Result =
            {
                result.Select(x => x.ToResponse())
            }
        };
    }

    public override async Task<GetCountyByIdResponse> GetById(GetCountyByIdRequest request, ServerCallContext context)
    {
        var query = new GetCountyByIdQuery(request.VoivodeshipId, request.CountyId);

        var result = await mediator.Send(query);

        return new GetCountyByIdResponse
        {
            Result = result.ToResponse()
        };
    }
}