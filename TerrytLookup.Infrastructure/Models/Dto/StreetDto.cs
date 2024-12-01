namespace TerrytLookup.Infrastructure.Models.Dto;

public class StreetDto
{
    public required string Name { get; init; }

    public int NameId { get; init; }
    
    public int TownId { get; init; }

    public required TownDto Town { get; init; }
}