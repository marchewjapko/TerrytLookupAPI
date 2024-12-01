using FizzWare.NBuilder;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Tests.RepositoryTests;

[Parallelizable(ParallelScope.Self)]
public class VoivodeshipRepositoryTests
{
    private static readonly AppDbContext Context = TestContextSetup.SetupAsync()
        .Result;

    private static readonly VoivodeshipRepository Repository = new(Context);

    [SetUp]
    public void Setup()
    {
        Assume.That(Context.Voivodeships, Is.Empty);
    }

    [TearDown]
    public void TearDown()
    {
        Context.Voivodeships.RemoveRange(Context.Voivodeships);
        Context.SaveChanges();
    }

    [Test]
    public async Task AddRangeAsync_ShouldAddRange()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build();

        //Act
        await Repository.AddRangeAsync(voivodeships);

        //Assert
        Assert.That(Context.Voivodeships.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnTrue()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build();

        await Repository.AddRangeAsync(voivodeships);

        Assume.That(Context.Voivodeships.Count(), Is.EqualTo(10));

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
    public async Task GetByIdAsync_ShouldReturnVoivodeship()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build();

        await Repository.AddRangeAsync(voivodeships);

        Assume.That(Context.Voivodeships.Count(), Is.EqualTo(10));

        //Act
        var result = await Repository.GetByIdAsync(voivodeships.First()
            .Id);

        //Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(voivodeships.First()
                .Id));
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

    [Test]
    public async Task BrowseAllAsync_ShouldReturnVoivodeships()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build();

        await Repository.AddRangeAsync(voivodeships);

        Assume.That(Context.Voivodeships.Count(), Is.EqualTo(10));

        //Act
        var result = await Repository.BrowseAllAsync()
            .ToListAsync();

        //Assert
        Assert.That(result, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnEmptyCollection()
    {
        //Act
        var result = await Repository.BrowseAllAsync()
            .ToListAsync();

        //Assert
        Assert.That(result, Is.Empty);
    }
}