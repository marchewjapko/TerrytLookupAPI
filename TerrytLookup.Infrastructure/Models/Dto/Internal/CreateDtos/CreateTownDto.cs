using System.Collections.Concurrent;

namespace TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

public sealed class CreateTownDto
{
    public required int TerrytId { get; init; }

    public required string Name { get; init; }

    public required (int voivodeshipId, int countyId) CountyTerrytId { get; init; }

    public required int ParentTownTerrytId { get; init; }

    public required DateOnly ValidFromDate { get; init; }

    public CreateTownDto? ParentTown { get; set; }

    public ConcurrentBag<CreateStreetDto> Streets { get; init; } = [];

    public CreateCountyDto? County { get; set; }

    public void CopyStreetsTo(CreateTownDto town)
    {
        foreach (var street in Streets) town.Streets.Add(street);
    }
}