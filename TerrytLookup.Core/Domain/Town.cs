using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name))]
[UsedImplicitly]
public class Town : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(100)]
    public required string NormalizedName { get; set; }

    public int? ParentTownId { get; init; }

    [ForeignKey("ParentTownId")]
    public virtual Town? ParentTown { get; init; }

    public int CountyId { get; init; }

    public int CountyVoivodeshipId { get; init; }

    [ForeignKey(nameof(CountyVoivodeshipId) + "," + nameof(CountyId))]
    [Required]
    public virtual County County { get; init; } = null!;


    public virtual ICollection<Street> Streets { get; init; } = new List<Street>();
}