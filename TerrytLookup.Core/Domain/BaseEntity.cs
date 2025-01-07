using JetBrains.Annotations;

namespace TerrytLookup.Core.Domain;

public class BaseEntity
{
    [UsedImplicitly] public DateOnly ValidFromDate { get; init; }

    public DateTimeOffset CreateTimestamp { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdateTimestamp { get; set; }
}