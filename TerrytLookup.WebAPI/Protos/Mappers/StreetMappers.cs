using TerrytLookup.Core.Domain;
using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.WebAPI.Protos.Mappers;

public static class StreetMappers
{
    public static StreetResponse ToResponse(this StreetDto street)
    {
        var result = new StreetResponse
        {
            Name = street.Name,
            NameId = street.NameId,
            TownId = street.TownId,
            Town = street.Town.ToResponse()
        };

        return result;
    }
}