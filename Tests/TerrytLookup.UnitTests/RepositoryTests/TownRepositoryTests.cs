using Bogus;
using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Specifications;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.UnitTests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class TownRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    private static readonly TownRepository Repository = new(Context);

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
        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        //Act
        await Repository.AddRangeAsync(towns);

        //Assert
        Assert.That(Context.Towns.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

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
        const int pageSize = 10;

        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        var specification = new TownGetByFilterSpecification(pageSize);

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
        const string townName = "ThisOne!";
        const int pageSize = 10;

        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        var validTown = new Faker<Town>()
            .RuleFor(x => x.Id, _ => towns.Max(x => x.Id) + 1)
            .RuleFor(x => x.Name, _ => townName)
            .RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .Generate();

        towns.Add(validTown);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        var specification = new TownGetByFilterSpecification(pageSize, townName);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].Name, Is.EqualTo(townName));
                Assert.That(result[0].Id, Is.EqualTo(validTown.Id));
            });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByVoivodeship()
    {
        //Arrange
        const int pageSize = 10;

        var newVoivodeship = new Faker<Voivodeship>()
            .RuleFor(x => x.Id, _ => 3)
            .RuleFor(x => x.Name, f => f.Address.State())
            .Generate();

        var newCounty = new Faker<County>()
            .RuleFor(x => x.VoivodeshipId, _ => newVoivodeship.Id)
            .RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.Name, f => f.Address.County())
            .Generate();

        Context.Voivodeships.Add(newVoivodeship);
        Context.Counties.Add(newCounty);

        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        var validTown = new Faker<Town>()
            .RuleFor(x => x.Id, _ => towns.Max(x => x.Id) + 1)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.CountyVoivodeshipId, _ => 3)
            .Generate();

        towns.Add(validTown);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        var specification = new TownGetByFilterSpecification(pageSize, voivodeshipId: newVoivodeship.Id);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].Name, Is.EqualTo(validTown.Name));
                Assert.That(result[0].Id, Is.EqualTo(validTown.Id));
                Assert.That(result[0].CountyVoivodeshipId, Is.EqualTo(newVoivodeship.Id));
            });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByCounty()
    {
        //Arrange
        const string selectedTownName = "ThisOne!";
        const int pageSize = 10;

        var newVoivodeship = new Faker<Voivodeship>()
            .RuleFor(x => x.Id, _ => 3)
            .RuleFor(x => x.Name, f => f.Address.State())
            .Generate();

        var newCounties = new Faker<County>()
            .RuleFor(x => x.VoivodeshipId, _ => newVoivodeship.Id)
            .RuleFor(x => x.CountyId, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.County())
            .Generate(3);

        Context.Voivodeships.Add(newVoivodeship);
        Context.Counties.AddRange(newCounties);

        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        var moreInvalidTown = new Faker<Town>()
            .RuleFor(x => x.Id, _ => towns.Max(x => x.Id) + 1)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        var validTown = new Faker<Town>()
            .RuleFor(x => x.Id, _ => moreInvalidTown.Id + 1)
            .RuleFor(x => x.Name, _ => selectedTownName)
            .RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.CountyVoivodeshipId, _ => 3)
            .Generate();

        towns.AddRange(moreInvalidTown, validTown);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        var specification = new TownGetByFilterSpecification(
            pageSize,
            voivodeshipId: newVoivodeship.Id,
            countyId: validTown.CountyId);

        //Act
        var result = await Repository
            .BrowseAllAsync(specification)
            .ToListAsync();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].Name, Is.EqualTo(selectedTownName));
                Assert.That(result[0].Id, Is.EqualTo(validTown.Id));
                Assert.That(result[0].CountyVoivodeshipId, Is.EqualTo(newVoivodeship.Id));
                Assert.That(result[0].CountyId, Is.EqualTo(validTown.CountyId));
            });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnTown()
    {
        //Arrange
        var towns = new Faker<Town>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.City())
            .RuleFor(x => x.CountyVoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate(10);

        Context.AddRange(towns);
        await Context.SaveChangesAsync();

        var specification = new TownGetByIdSpecification(towns[0].Id);

        //Act
        var result = await Repository.FirstOrDefaultAsync(specification);

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(towns[0].Id));
            });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        //Arrange
        var specification = new TownGetByIdSpecification(1);

        //Act
        var result = await Repository.FirstOrDefaultAsync(specification);

        //Assert
        Assert.That(result, Is.Null);
    }
}