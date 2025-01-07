using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Models.Mappers;

public static class TownMappers
{
    public static Town ToDomain(this SimcDto simcDto)
    {
        return new Town
        {
            Id = simcDto.Id,
            Name = simcDto.Name,
            CountyId = simcDto.CountyId,
            CountyVoivodeshipId = simcDto.VoivodeshipId,
            ValidFromDate = simcDto.ValidFromDate,
            ParentTownId = simcDto.ParentId == simcDto.Id ? null : simcDto.ParentId
        };
    }

    public static TownDto ToDto(this Town town)
    {
        if (town.ParentTown is not null)
            return new TownDto
            {
                Id = town.ParentTown.Id,
                Name = town.ParentTown.Name,
                County = town.ParentTown.County.ToDto()
            };

        return new TownDto
        {
            Id = town.Id,
            Name = town.Name,
            County = town.County.ToDto()
        };
    }
}