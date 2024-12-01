using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Tests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class StreetRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    private static readonly StreetRepository Repository = new(Context);

    [SetUp]
    public void Setup()
    {
        Context.Voivodeships.Add(Builder<Voivodeship>.CreateNew()
            .Build());
        Context.Counties.Add(Builder<County>.CreateNew()
            .Build());
        Context.Towns.Add(Builder<Town>.CreateNew()
            .Build());
        Context.SaveChanges();

        Assume.That(Context.Counties, Is.Not.Empty);
        Assume.That(Context.Voivodeships, Is.Not.Empty);
        Assume.That(Context.Towns, Is.Not.Empty);
        Assume.That(Context.Streets, Is.Empty);
    }

    [TearDown]
    public void TearDown()
    {
        Context.Voivodeships.RemoveRange(Context.Voivodeships);
        Context.Counties.RemoveRange(Context.Counties);
        Context.Towns.RemoveRange(Context.Towns);
        Context.Streets.RemoveRange(Context.Streets);
        Context.SaveChanges();
    }

    [Test]
    public async Task AddRangeAsync_ShouldAddRange()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .Build();

        //Act
        await Repository.AddRangeAsync(streets);

        //Assert
        Assert.That(Context.Streets.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .Build();

        Context.AddRange(streets);
        Context.SaveChanges();

        Assume.That(Context.Streets, Is.Not.Empty);

        //Act
        var result = await Repository.ExistAnyAsync();

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnFalse()
    {
        //Act
        var result = await Repository.ExistAnyAsync();

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnAllStreets()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .Build();

        Context.AddRange(streets);
        Context.SaveChanges();

        //Act
        var result = await Repository.BrowseAllAsync()
            .ToListAsync();

        //Assert
        Assert.That(result, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByName()
    {
        //Arrange
        const string streetName = "ThisOne!";

        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .TheFirst(1)
            .With(x => x.Name = streetName)
            .TheRest()
            .With(x => x.Name = "NotThisOne:/")
            .Build();

        Context.AddRange(streets);
        Context.SaveChanges();

        //Act
        var result = await Repository.BrowseAllAsync(streetName)
            .ToListAsync();

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(streetName));
            Assert.That(result[0].TownId, Is.EqualTo(streets[0].TownId));
            Assert.That(result[0].NameId, Is.EqualTo(streets[0].NameId));
        });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByTownId()
    {
        //Arrange
        var newTown = Builder<Town>.CreateNew()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .With(x => x.Id = 3)
            .Build();
        Context.Towns.Add(newTown);

        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .TheFirst(7)
            .With(x => x.TownId = newTown.Id)
            .Build();

        Context.AddRange(streets);
        Context.SaveChanges();

        //Act
        var result = await Repository.BrowseAllAsync(townId: newTown.Id)
            .ToListAsync();

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Has.Count.EqualTo(7));
            Assert.That(result.All(x => x.TownId == newTown.Id), Is.True);
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnStreet()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .All()
            .With(x => x.TownId = 1)
            .Build();

        Context.AddRange(streets);
        Context.SaveChanges();

        //Act
        var result = await Repository.GetByIdAsync(streets[0].TownId, streets[0].NameId);

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.TownId, Is.EqualTo(streets[0].TownId));
            Assert.That(result.NameId, Is.EqualTo(streets[0].NameId));
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        //Act
        var result = await Repository.GetByIdAsync(1, 1);

        //Assert
        Assert.That(result, Is.Null);
    }
}