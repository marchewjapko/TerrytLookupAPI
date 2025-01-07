using Bogus;
using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Tests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class CountyRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync().Result;

    private static readonly CountyRepository Repository = new(Context);

    [SetUp]
    public void Setup()
    {
        Context.Voivodeships.Add(Builder<Voivodeship>.CreateNew().Build());

        Context.SaveChanges();

        Assume.That(Context.Counties, Is.Empty);
        Assume.That(Context.Voivodeships, Is.Not.Empty);
    }

    [TearDown]
    public void TearDown()
    {
        Context.Voivodeships.RemoveRange(Context.Voivodeships);
        Context.Counties.RemoveRange(Context.Counties);
        Context.SaveChanges();
    }

    [Test]
    public async Task AddRangeAsync_ShouldAddRange()
    {
        //Arrange
        var counties = new Faker<County>().RuleFor(x => x.CountyId, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.County()).RuleFor(x => x.VoivodeshipId, _ => 1).Generate(10);

        //Act
        await Repository.AddRangeAsync(counties);

        //Assert
        Assert.That(Context.Counties.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var counties = new Faker<County>().RuleFor(x => x.CountyId, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.County()).RuleFor(x => x.VoivodeshipId, _ => 1).Generate(10);

        Context.AddRange(counties);
        await Context.SaveChangesAsync();

        Assume.That(Context.Counties, Is.Not.Empty);

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
    public async Task BrowseAllAsync_ShouldReturnAllCounties()
    {
        //Arrange
        var counties = new Faker<County>().RuleFor(x => x.CountyId, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.County()).RuleFor(x => x.VoivodeshipId, _ => 1).Generate(10);

        Context.AddRange(counties);
        await Context.SaveChangesAsync();

        Assume.That(Context.Counties, Is.Not.Empty);

        //Act
        var result = await Repository.BrowseAllAsync().ToListAsync();

        //Assert
        Assert.That(result, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByName()
    {
        //Arrange
        const string filterName = "ThisOne!";

        var counties = new Faker<County>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, f => f.IndexFaker).RuleFor(x => x.Name, f => f.Address.County()).Generate(10);

        var validCounty = new Faker<County>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.Name, _ => filterName).RuleFor(x => x.CountyId, _ => counties.Max(x => x.CountyId) + 1)
            .Generate();

        counties.Add(validCounty);

        Context.AddRange(counties);
        await Context.SaveChangesAsync();

        Assume.That(Context.Counties, Is.Not.Empty);

        //Act
        var result = await Repository.BrowseAllAsync(filterName).ToListAsync();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(filterName));
            Assert.That(result[0].CountyId, Is.EqualTo(validCounty.CountyId));
        });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldFilterByVoivodeshipId()
    {
        //Arrange
        var newVoivodeship = new Faker<Voivodeship>().RuleFor(x => x.Id, _ => 3)
            .RuleFor(x => x.Name, f => f.Address.State()).Generate();

        Context.Voivodeships.Add(newVoivodeship);

        var counties = new Faker<County>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, f => f.IndexFaker).RuleFor(x => x.Name, f => f.Address.County()).Generate(7);

        var validCounties = new Faker<County>().RuleFor(x => x.VoivodeshipId, _ => 3)
            .RuleFor(x => x.CountyId, f => counties.Max(x => x.CountyId) + f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.County()).Generate(3);

        counties.AddRange(validCounties);

        Context.AddRange(counties);
        await Context.SaveChangesAsync();

        Assume.That(Context.Counties, Is.Not.Empty);
        Assume.That(Context.Voivodeships.Count(), Is.EqualTo(2));

        //Act
        var result = await Repository.BrowseAllAsync(voivodeshipId: newVoivodeship.Id).ToListAsync();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.All(x => x.VoivodeshipId == newVoivodeship.Id), Is.True);
        });
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnEmptyCollection()
    {
        //Act
        var result = await Repository.BrowseAllAsync().ToListAsync();

        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCounty()
    {
        //Arrange
        var counties = new Faker<County>().RuleFor(x => x.CountyId, f => f.IndexFaker)
            .RuleFor(x => x.VoivodeshipId, _ => 1).RuleFor(x => x.Name, f => f.Address.County()).Generate(10);

        Context.AddRange(counties);
        await Context.SaveChangesAsync();

        //Act
        var result = await Repository.GetByIdAsync(counties.First().VoivodeshipId, counties.First().CountyId);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.CountyId, Is.EqualTo(counties.First().CountyId));
            Assert.That(result.VoivodeshipId, Is.EqualTo(counties.First().VoivodeshipId));
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