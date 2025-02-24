using Grpc.Core;
using MediatR;
using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Queries.Voivodeships.BrowseVoivodeships;
using TerrytLookup.UseCases.Queries.Voivodeships.GetVoivodeshipById;
using TerrytLookup.WebAPI.Protos.Mappers;

namespace TerrytLookup.WebAPI.Services;

public class VoivodeshipService(IMediator mediator) : VoivodeshipsApiService.VoivodeshipsApiServiceBase
{
    public override async Task<BrowseAllVoivodeshipsResponse> BrowseAll(
        BrowseAllVoivodeshipsRequest request,
        ServerCallContext context)
    {
        var query = new BrowseVoivodeshipsQuery();

        var result = await mediator
            .CreateStream(query)
            .ToListAsync();

        return new BrowseAllVoivodeshipsResponse
        {
            Result =
            {
                result.Select(x => x.ToResponse())
            }
        };
    }

    public override async Task<GetVoivodeshipByIdResponse> GetById(
        GetVoivodeshipByIdRequest request,
        ServerCallContext context)
    {
        var query = new GetVoivodeshipByIdQuery(request.Id);

        var result = await mediator.Send(query);

        return new GetVoivodeshipByIdResponse
        {
            Result = result.ToResponse()
        };
    }
}