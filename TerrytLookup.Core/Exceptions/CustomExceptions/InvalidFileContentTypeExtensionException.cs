using Grpc.Core;

namespace TerrytLookup.Core.Exceptions.CustomExceptions;

public class InvalidFileContentTypeExtensionException(string contentType)
    : Exception($"Unable to read file with content type of '{contentType}'."), ICustomMappedException
{
    // public ProblemDetails GetProblemDetails(Exception exception)
    // {
    //     return new ProblemDetails
    //     {
    //         Title = "Invalid content type.",
    //         Detail = exception.Message,
    //         Status = StatusCodes.Status400BadRequest
    //     };
    // }
    public RpcException ToRpcException()
    {
        throw new NotImplementedException();
    }
}