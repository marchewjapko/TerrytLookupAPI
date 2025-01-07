using JetBrains.Annotations;

namespace TerrytLookup.Infrastructure.Models.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TownDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required CountyDto County { get; init; }
}