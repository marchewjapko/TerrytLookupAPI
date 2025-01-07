using Bogus;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.TownService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class TownServiceTests
{
    private static readonly Mock<ITownRepository> TownRepository = new();
    private static readonly TownService TownService = new(TownRepository.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var simcDtos = Builder<SimcDto>.CreateListOfSize(10).Build();

        //Act
        await TownService.AddRange(simcDtos);

        //Assert
        TownRepository.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<Town>>()), Times.Once);
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnAllTowns()
    {
        //Arrange
        const int townSize = 10;

        var voivodeship = Builder<Voivodeship>.CreateNew().Build();

        var county = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate();

        var towns = new Faker<Town>().RuleFor(x => x.County, _ => county).Generate(townSize).ToAsyncEnumerable();

        TownRepository.Setup(x => x.BrowseAllAsync(null, null, null)).Returns(towns);

        //Act
        var result = await TownService.BrowseAllAsync().ToListAsync();

        //Assert
        TownRepository.Verify(x => x.BrowseAllAsync(null, null, null), Times.Once);
        Assert.That(result, Has.Count.EqualTo(townSize));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnTown()
    {
        //Arrange
        var voivodeship = Builder<Voivodeship>.CreateNew().Build();

        var county = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate();

        var town = new Faker<Town>().RuleFor(x => x.County, _ => county).Generate();

        TownRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(town);

        //Act
        var result = await TownService.GetByIdAsync(1);

        //Assert
        TownRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
        Assert.That(result.Id, Is.EqualTo(town.Id));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowTownNotFoundException()
    {
        //Arrange
        TownRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Town?)null);

        //Assert
        var exception = Assert.ThrowsAsync<TownNotFoundException>(() => TownService.GetByIdAsync(2));

        Assert.That(exception.Message, Is.EqualTo("Town with id 2 not found."));
        TownRepository.Verify(x => x.GetByIdAsync(2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        TownRepository.Setup(x => x.ExistAnyAsync()).ReturnsAsync(true);

        //Act
        var result = await TownService.ExistAnyAsync();

        //Assert
        TownRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}