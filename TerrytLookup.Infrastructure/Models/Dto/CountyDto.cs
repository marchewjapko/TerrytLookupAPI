namespace TerrytLookup.Infrastructure.Models.Dto;

public class CountyDto
{
    public required string Name { get; init; }

    public int VoivodeshipId { get; init; }

    public int CountyId { get; init; }
}