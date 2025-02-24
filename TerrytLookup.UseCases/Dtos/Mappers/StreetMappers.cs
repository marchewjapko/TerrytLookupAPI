using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Dtos.Mappers;

public static class StreetMappers
{
    public static Street ToDomain(this UlicDto ulicDto)
    {
        string?[] nameParts = [ulicDto.StreetPrefix, ulicDto.StreetNameSecondPart, ulicDto.StreetNameFirstPart];

        var name = string.Join(" ", nameParts.Where(part => !string.IsNullOrEmpty(part)));

        return new Street
        {
            Name = name,
            NormalizedName = name.NormalizeName(),
            NameId = ulicDto.StreetNameId,
            TownId = ulicDto.TownId,
            ValidFromDate = ulicDto.ValidFromDate
        };
    }

    public static StreetDto ToDto(this Street street)
    {
        return new StreetDto(
            street.Name,
            street.NameId,
            street.Town.ParentTown?.Id ?? street.Town.Id,
            street.Town.ParentTown?.ToDto() ?? street.Town.ToDto());
    }
}