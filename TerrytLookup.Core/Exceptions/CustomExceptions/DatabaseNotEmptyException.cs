using Grpc.Core;

namespace TerrytLookup.Core.Exceptions.CustomExceptions;

public class DatabaseNotEmptyException(IEnumerable<string> repositories) : Exception(
        $"Unable to initialize registers. The following repositories are not empty: {string.Join(", ", repositories)}"),
    ICustomMappedException
{
    // public ProblemDetails GetProblemDetails(Exception exception)
    // {
    //     return new ProblemDetails
    //     {
    //         Title = "Unable to initialize registers.",
    //         Detail = exception.Message,
    //         Status = StatusCodes.Status409Conflict
    //     };
    // }
    public RpcException ToRpcException()
    {
        throw new NotImplementedException();
    }
}