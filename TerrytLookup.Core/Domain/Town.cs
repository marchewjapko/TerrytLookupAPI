using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name))]
public class Town : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public required string Name { get; set; }

    [ForeignKey("ParentTownId")]
    public Town? ParentTown { get; set; }

    [ForeignKey(nameof(CountyVoivodeshipId) + "," + nameof(CountyId))]
    public required County County { get; set; }
    
    public int CountyVoivodeshipId { get; set; }

    public int CountyId { get; set; }

    public required ICollection<Street> Streets { get; set; }
}