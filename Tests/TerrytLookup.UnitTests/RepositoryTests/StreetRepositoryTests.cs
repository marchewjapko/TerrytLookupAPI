using Bogus;
using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Specifications;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.UnitTests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class StreetRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    private static readonly StreetRepository Repository = new(Context);

    [SetUp]
    public void Setup()
    {
        Context.Voivodeships.Add(
            Builder<Voivodeship>
                .CreateNew()
                .Build());

        Context.Counties.Add(
            Builder<County>
                .CreateNew()
                .Build());

        Context.Towns.Add(
            Builder<Town>
                .CreateNew()
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
        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .Generate(10);

        //Act
        await Repository.AddRangeAsync(streets);

        //Assert
        Assert.That(Context.Streets.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .Generate(10);

        Context.AddRange(streets);
        await Context.SaveChangesAsync();

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
        const int pageSize = 10;

        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .Generate(10);

        Context.AddRange(streets);
        await Context.SaveChangesAsync();

        var specification = new StreetGetByFilterSpecification(pageSize);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.That(result, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByName()
    {
        //Arrange
        const int pageSize = 10;
        const string streetName = "ThisOne!";

        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .Generate(10);

        streets.Add(
            new Street
            {
                TownId = 1,
                Name = streetName,
                NameId = streets.Max(x => x.NameId) + 1
            });

        Context.AddRange(streets);
        await Context.SaveChangesAsync();

        var specification = new StreetGetByFilterSpecification(pageSize, streetName);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].Name, Is.EqualTo(streetName));
                Assert.That(result[0].TownId, Is.EqualTo(streets[^1].TownId));
                Assert.That(result[0].NameId, Is.EqualTo(streets[^1].NameId));
            });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByTownId()
    {
        //Arrange
        const int pageSize = 10;

        var newTown = new Faker<Town>()
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        Context.Towns.Add(newTown);

        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .Generate(3);

        var validStreets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => streets.Max(x => x.NameId) + f.IndexFaker)
            .RuleFor(x => x.TownId, _ => newTown.Id)
            .Generate(7);

        streets.AddRange(validStreets);

        Context.AddRange(streets);
        await Context.SaveChangesAsync();

        var specification = new StreetGetByFilterSpecification(pageSize, townId: newTown.Id);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Has.Count.EqualTo(7));
                Assert.That(result.All(x => x.TownId == newTown.Id), Is.True);
            });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnStreet()
    {
        //Arrange
        var streets = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, _ => 1)
            .Generate(10);

        Context.AddRange(streets);
        await Context.SaveChangesAsync();

        var specification = new StreetGetByIdSpecification(streets[0].TownId, streets[0].NameId);

        //Act
        var result = await Repository.FirstOrDefaultAsync(specification);

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.TownId, Is.EqualTo(streets[0].TownId));
                Assert.That(result.NameId, Is.EqualTo(streets[0].NameId));
            });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        //Arrange
        var specification = new StreetGetByIdSpecification(1, 1);

        //Act
        var result = await Repository.FirstOrDefaultAsync(specification);

        //Assert
        Assert.That(result, Is.Null);
    }
}