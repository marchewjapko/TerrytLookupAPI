using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Models.Mappers;

public static class CountyMappers
{
    public static County ToDomainCounty(this TercDto tercDto)
    {
        if (tercDto.CountyId is null) throw new Exception();

        return new County
        {
            CountyId = tercDto.CountyId.Value,
            VoivodeshipId = tercDto.VoivodeshipId,
            Name = tercDto.Name,
            ValidFromDate = tercDto.ValidFromDate
        };
    }

    public static CountyDto ToDto(this County county)
    {
        return new CountyDto
        {
            Name = county.Name,
            Voivodeship = county.Voivodeship.ToDto(),
            CountyId = county.CountyId,
            VoivodeshipId = county.VoivodeshipId
        };
    }
}