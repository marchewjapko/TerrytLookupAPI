using JetBrains.Annotations;

namespace TerrytLookup.UseCases.Dtos.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record CountyDto(
    string Name,
    int CountyId,
    int VoivodeshipId,
    VoivodeshipDto Voivodeship);