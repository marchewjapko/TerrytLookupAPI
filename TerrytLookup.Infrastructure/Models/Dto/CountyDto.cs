using JetBrains.Annotations;

namespace TerrytLookup.Infrastructure.Models.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CountyDto
{
    public required string Name { get; init; }

    public int VoivodeshipId { get; init; }

    public int CountyId { get; init; }

    public required VoivodeshipDto Voivodeship { get; init; }
}