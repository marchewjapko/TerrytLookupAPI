using Grpc.Core;

namespace TerrytLookup.Core.Exceptions.CustomExceptions;

public class InvalidDatabaseConfigurationException(string error) : Exception(error), ICustomMappedException
{
    // public ProblemDetails GetProblemDetails(Exception exception)
    // {
    //     return new ProblemDetails
    //     {
    //         Title = "Invalid database configuration.",
    //         Detail = exception.Message,
    //         Status = StatusCodes.Status500InternalServerError
    //     };
    // }
    public RpcException ToRpcException()
    {
        throw new NotImplementedException();
    }
}