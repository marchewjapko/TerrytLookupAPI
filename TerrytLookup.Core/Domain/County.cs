using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TerrytLookup.Core.Domain;

[Index(nameof(Name))]
[Index(nameof(VoivodeshipId), nameof(CountyId), IsUnique = true)]
[PrimaryKey(nameof(VoivodeshipId), nameof(CountyId))]
public class County : BaseEntity
{
    public int VoivodeshipId { get; set; }

    public int CountyId { get; set; }

    public required string Name { get; set; }

    public required Voivodeship Voivodeship { get; set; }

    public required ICollection<Town> Towns { get; set; }
}