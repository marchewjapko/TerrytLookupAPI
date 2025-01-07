using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Models.Mappers;

public static class StreetMappers
{
    public static Street ToDomain(this UlicDto ulicDto)
    {
        string?[] nameParts = [ulicDto.StreetPrefix, ulicDto.StreetNameSecondPart, ulicDto.StreetNameFirstPart];

        return new Street
        {
            Name = string.Join(" ", nameParts.Where(part => !string.IsNullOrEmpty(part))),
            NameId = ulicDto.StreetNameId,
            TownId = ulicDto.TownId,
            ValidFromDate = ulicDto.ValidFromDate
        };
    }

    public static StreetDto ToDto(this Street street)
    {
        if (street.Town.ParentTown is not null)
            return new StreetDto
            {
                Name = street.Name,
                Town = street.Town.ParentTown.ToDto(),
                NameId = street.NameId,
                TownId = street.Town.ParentTown.Id
            };

        return new StreetDto
        {
            Name = street.Name,
            Town = street.Town.ToDto(),
            NameId = street.NameId,
            TownId = street.TownId
        };
    }
}