using Bogus;
using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Tests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class TownRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    private static readonly TownRepository Repository = new(Context);

    [SetUp]
    public void Setup()
    {
        Context.Voivodeships.Add(Builder<Voivodeship>.CreateNew()
            .Build());
        Context.Counties.Add(Builder<County>.CreateNew()
            .Build());
        Context.SaveChanges();

        Assume.That(Context.Counties, Is.Not.Empty);
        Assume.That(Context.Voivodeships, Is.Not.Empty);
        Assume.That(Context.Towns, Is.Empty);
    }

    [TearDown]
    public void TearDown()
    {
        Context.Voivodeships.RemoveRange(Context.Voivodeships);
        Context.Counties.RemoveRange(Context.Counties);
        Context.Towns.RemoveRange(Context.Towns);
        Context.SaveChanges();
    }

    [Test]
    public async Task AddRangeAsync_ShouldAddRange()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .Build();

        //Act
        await Repository.AddRangeAsync(towns);

        //Assert
        Assert.That(Context.Towns.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .Build();

        Context.AddRange(towns);
        Context.SaveChanges();

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
    public async Task BrowseAllAsync_ShouldReturnAll()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .Build();

        Context.AddRange(towns);
        Context.SaveChanges();

        //Act
        var result = await Repository.BrowseAllAsync()
            .ToListAsync();

        //Assert
        Assert.That(result.Count, Is.EqualTo(10));
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByName()
    {
        //Arrange
        const string townName = "ThisOne!";

        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .TheFirst(1)
            .With(x => x.Name = townName)
            .TheRest()
            .With(x => x.Name = "NotThisOne:/")
            .Build();

        Context.AddRange(towns);
        Context.SaveChanges();

        //Act
        var result = await Repository.BrowseAllAsync(townName)
            .ToListAsync();

        //Assert
        Assert.Multiple(() => {
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(townName));
            Assert.That(result[0].Id, Is.EqualTo(towns[0].Id));
        });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByVoivodeship()
    {
        //Arrange
        var newVoivodeship = Builder<Voivodeship>.CreateNew()
            .With(x => x.Id = 3)
            .With(x => x.Name = new Faker().Random.Word())
            .Build();
        var newCounty = Builder<County>.CreateNew()
            .With(x => x.VoivodeshipId = newVoivodeship.Id)
            .With(x => x.CountyId = 1)
            .With(x => x.Name = new Faker().Random.Word())
            .Build();
        Context.Voivodeships.Add(newVoivodeship);
        Context.Counties.Add(newCounty);

        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .TheFirst(1)
            .With(x => x.CountyVoivodeshipId = 3)
            .With(x => x.CountyId = 1)
            .Build();

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        //Act
        var result = await Repository.BrowseAllAsync(voivodeshipId: newVoivodeship.Id)
            .ToListAsync();

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(towns[0].Name));
            Assert.That(result[0].Id, Is.EqualTo(towns[0].Id));
            Assert.That(result[0].CountyVoivodeshipId, Is.EqualTo(newVoivodeship.Id));
        });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByCounty()
    {
        //Arrange
        var newVoivodeship = Builder<Voivodeship>.CreateNew()
            .With(x => x.Id = 3)
            .With(x => x.Name = new Faker().Random.Word())
            .Build();
        var newCounties = Builder<County>.CreateListOfSize(3)
            .All()
            .With(x => x.VoivodeshipId = newVoivodeship.Id)
            .With(x => x.Name = new Faker().Random.Word())
            .Build();
        Context.Voivodeships.Add(newVoivodeship);
        Context.Counties.AddRange(newCounties);

        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .TheFirst(1)
            .With(x => x.CountyVoivodeshipId = 3)
            .With(x => x.CountyId = 1)
            .Build();

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        //Act
        var result = await Repository.BrowseAllAsync(voivodeshipId: newVoivodeship.Id, countyId: newCounties[0].CountyId)
            .ToListAsync();

        //Assert
        Assert.Multiple(() => {
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(towns[0].Name));
            Assert.That(result[0].Id, Is.EqualTo(towns[0].Id));
            Assert.That(result[0].CountyVoivodeshipId, Is.EqualTo(newVoivodeship.Id));
            Assert.That(result[0].CountyId, Is.EqualTo(newCounties[0].CountyId));
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnTown()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .All()
            .With(x => x.CountyVoivodeshipId = 1)
            .With(x => x.CountyId = 1)
            .Build();

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        //Act
        var result = await Repository.GetByIdAsync(towns[0].Id);

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(towns[0].Id));
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        //Act
        var result = await Repository.GetByIdAsync(1);

        //Assert
        Assert.That(result, Is.Null);
    }
}