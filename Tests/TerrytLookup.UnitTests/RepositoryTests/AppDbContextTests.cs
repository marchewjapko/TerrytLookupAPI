using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.UnitTests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class AppDbContextTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    [Test]
    public async Task SaveChanges_ShouldImplicitSetUpdateTs()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>
            .CreateListOfSize(10)
            .Build();

        Context.AddRange(voivodeships);
        await Context.SaveChangesAsync();

        Assume.That(Context.Voivodeships, Is.Not.Empty);
        Assume.That(Context.Voivodeships.All(x => x.UpdateTimestamp == null), Is.True);

        //Act
        var voivodeshipsToUpdate = Context
            .Voivodeships.AsTracking()
            .ToList();

        foreach (var voivodeship in voivodeshipsToUpdate.Take(4))
            voivodeship.Name += "Updated";

        await Context.SaveChangesAsync();

        //Assert
        Assert.That(Context.Voivodeships.Count(x => x.UpdateTimestamp != null), Is.EqualTo(4));
    }
}