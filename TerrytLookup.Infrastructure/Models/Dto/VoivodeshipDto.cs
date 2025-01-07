using JetBrains.Annotations;

namespace TerrytLookup.Infrastructure.Models.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class VoivodeshipDto
{
    public int Id { get; init; }

    public required string Name { get; init; }
}