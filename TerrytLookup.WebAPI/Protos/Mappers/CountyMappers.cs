using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.WebAPI.Protos.Mappers;

public static class CountyMappers
{
    public static CountyResponse ToResponse(this CountyDto county)
    {
        var result = new CountyResponse
        {
            Name = county.Name,
            CountyId = county.CountyId,
            VoivodeshipId = county.VoivodeshipId,
            Voivodeship = county.Voivodeship.ToResponse()
        };

        return result;
    }
}