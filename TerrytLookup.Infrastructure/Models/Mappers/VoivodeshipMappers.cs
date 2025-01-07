using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Models.Mappers;

public static class VoivodeshipMappers
{
    public static Voivodeship ToDomainVoivodeship(this TercDto tercDto)
    {
        return new Voivodeship
        {
            Id = tercDto.VoivodeshipId,
            Name = tercDto.Name,
            ValidFromDate = tercDto.ValidFromDate
        };
    }

    public static VoivodeshipDto ToDto(this Voivodeship voivodeship)
    {
        return new VoivodeshipDto
        {
            Id = voivodeship.Id,
            Name = voivodeship.Name
        };
    }
}