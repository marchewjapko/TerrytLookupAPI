using JetBrains.Annotations;

namespace TerrytLookup.UseCases.Dtos.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record StreetDto(
    string Name,
    int NameId,
    int TownId,
    TownDto Town);