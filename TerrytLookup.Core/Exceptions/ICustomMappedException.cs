using Grpc.Core;

namespace TerrytLookup.Core.Exceptions;

public interface ICustomMappedException
{
    public RpcException ToRpcException();
}