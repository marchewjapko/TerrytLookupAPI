using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.WebAPI.Protos.Mappers;

public static class TownMappers
{
    public static TownResponse ToResponse(this TownDto town)
    {
        var result = new TownResponse
        {
            Id = town.Id,
            Name = town.Name,
            County = town.County.ToResponse()
        };

        return result;
    }
}