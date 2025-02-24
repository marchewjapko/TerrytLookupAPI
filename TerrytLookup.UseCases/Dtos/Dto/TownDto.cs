using JetBrains.Annotations;

namespace TerrytLookup.UseCases.Dtos.Dto;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record TownDto(int Id, string Name, CountyDto County);