using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Helpers;
using TerrytLookup.UseCases.Dtos.Dto;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Dtos.Mappers;

public static class TownMappers
{
    public static Town ToDomain(this SimcDto simcDto)
    {
        return new Town
        {
            Id = simcDto.Id,
            Name = simcDto.Name,
            NormalizedName = simcDto.Name.NormalizeName(),
            CountyId = simcDto.CountyId,
            CountyVoivodeshipId = simcDto.VoivodeshipId,
            ValidFromDate = simcDto.ValidFromDate,
            ParentTownId = simcDto.ParentId == simcDto.Id ? null : simcDto.ParentId
        };
    }

    public static TownDto ToDto(this Town town)
    {
        return town.ParentTown is not null
            ? new TownDto(town.ParentTown.Id, town.ParentTown.Name, town.County.ToDto())
            : new TownDto(town.Id, town.Name, town.County.ToDto());
    }
}