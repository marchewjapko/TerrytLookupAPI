using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name))]
[PrimaryKey(nameof(VoivodeshipId), nameof(CountyId))]
[UsedImplicitly]
public class County : BaseEntity
{
    public int VoivodeshipId { get; init; }

    public int CountyId { get; init; }

    public required string Name { get; init; }

    [Required] public virtual Voivodeship Voivodeship { get; init; } = null!;

    public virtual ICollection<Town> Towns { get; init; } = new List<Town>();
}