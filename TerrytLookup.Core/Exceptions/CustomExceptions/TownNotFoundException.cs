using Grpc.Core;

namespace TerrytLookup.Core.Exceptions.CustomExceptions;

public class TownNotFoundException(int id) : Exception($"Town with id {id} not found."), ICustomMappedException
{
    // public ProblemDetails GetProblemDetails(Exception exception)
    // {
    //     return new ProblemDetails
    //     {
    //         Title = "Town not found.",
    //         Detail = exception.Message,
    //         Status = StatusCodes.Status404NotFound
    //     };
    // }
    public RpcException ToRpcException()
    {
        throw new NotImplementedException();
    }
}