using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name), IsUnique = true)]
[UsedImplicitly]
public class Voivodeship : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public required string NormalizedName { get; set; }

    public virtual ICollection<County> Counties { get; init; } = new List<County>();
}