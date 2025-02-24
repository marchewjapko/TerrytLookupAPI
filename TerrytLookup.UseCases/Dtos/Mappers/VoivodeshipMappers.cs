using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Dtos.Mappers;

public static class VoivodeshipMappers
{
    public static Voivodeship ToDomainVoivodeship(this TercDto tercDto)
    {
        return new Voivodeship
        {
            Id = tercDto.VoivodeshipId,
            Name = tercDto.Name,
            NormalizedName = tercDto.Name.NormalizeName(),
            ValidFromDate = tercDto.ValidFromDate
        };
    }

    public static VoivodeshipDto ToDto(this Voivodeship voivodeship)
    {
        return new VoivodeshipDto(voivodeship.Id, voivodeship.Name);
    }
}