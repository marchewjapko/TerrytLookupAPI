using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name))]
[PrimaryKey(nameof(TownId), nameof(NameId))]
[UsedImplicitly]
public class Street : BaseEntity
{
    public int NameId { get; init; }

    [MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(100)]
    public required string NormalizedName { get; set; }

    public int TownId { get; init; }

    [Required]
    public virtual Town Town { get; init; } = null!;
}