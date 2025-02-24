using TerrytLookup.gRPCServices;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.WebAPI.Protos.Mappers;

public static class VoivodeshipMappers
{
    public static VoivodeshipResponse ToResponse(this VoivodeshipDto voivodeship)
    {
        var result = new VoivodeshipResponse
        {
            Id = voivodeship.Id,
            Name = voivodeship.Name
        };

        return result;
    }
}