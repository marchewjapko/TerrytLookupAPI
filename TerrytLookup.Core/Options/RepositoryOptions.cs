namespace TerrytLookup.Core.Options;

public class RepositoryOptions
{
    public int VoivodeshipPageSize { get; init; } = 16;

    public int CountyPageSize { get; init; } = 10;

    public int TownPageSize { get; init; } = 10;

    public int StreetPageSize { get; init; } = 10;
}