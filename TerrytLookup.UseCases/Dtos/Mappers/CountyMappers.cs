using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Dtos.Mappers;

public static class CountyMappers
{
    public static County ToDomainCounty(this TercDto tercDto)
    {
        if (tercDto.CountyId is null)
            throw new Exception();

        return new County
        {
            CountyId = tercDto.CountyId.Value,
            VoivodeshipId = tercDto.VoivodeshipId,
            Name = tercDto.Name,
            NormalizedName = tercDto.Name.NormalizeName(),
            ValidFromDate = tercDto.ValidFromDate
        };
    }

    public static CountyDto ToDto(this County county)
    {
        return new CountyDto(
            county.Name,
            county.CountyId,
            county.VoivodeshipId,
            county.Voivodeship.ToDto());
    }
}