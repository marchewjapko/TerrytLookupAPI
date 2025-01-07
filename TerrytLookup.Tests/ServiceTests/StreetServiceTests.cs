using Bogus;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.StreetService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class StreetServiceTests
{
    private static readonly Mock<IStreetRepository> StreetRepository = new();
    private static readonly StreetService StreetService = new(StreetRepository.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var ulicDtos = Builder<UlicDto>.CreateListOfSize(10).Build();

        //Act
        await StreetService.AddRange(ulicDtos);

        //Assert
        StreetRepository.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<Street>>()), Times.Once);
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnAllStreets()
    {
        //Arrange
        const int streetSize = 10;
        var voivodeship = new Faker<Voivodeship>().Generate();

        var county = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate();

        var town = new Faker<Town>().RuleFor(x => x.County, _ => county).Generate();

        var streets = new Faker<Street>().RuleFor(x => x.Town, _ => town).Generate(streetSize).ToAsyncEnumerable();

        StreetRepository.Setup(x => x.BrowseAllAsync(null, null)).Returns(streets);

        //Act
        var result = await StreetService.BrowseAllAsync().ToListAsync();

        //Assert
        StreetRepository.Verify(x => x.BrowseAllAsync(null, null), Times.Once);
        Assert.That(result, Has.Count.EqualTo(streetSize));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnStreet()
    {
        //Arrange
        var voivodeship = new Faker<Voivodeship>().Generate();

        var county = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate();

        var town = new Faker<Town>().RuleFor(x => x.County, _ => county).Generate();

        var street = new Faker<Street>().RuleFor(x => x.Town, _ => town).Generate();

        StreetRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(street);

        //Act
        var result = await StreetService.GetByIdAsync(1, 1);

        //Assert
        StreetRepository.Verify(x => x.GetByIdAsync(1, 1), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result.NameId, Is.EqualTo(street.NameId));
            Assert.That(result.TownId, Is.EqualTo(street.TownId));
        });
    }

    [Test]
    public void GetByIdAsync_ShouldThrowStreetNotFoundException()
    {
        //Arrange
        StreetRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Street?)null);

        //Assert
        var exception = Assert.ThrowsAsync<StreetNotFoundException>(() => StreetService.GetByIdAsync(2, 2));

        Assert.That(exception.Message, Is.EqualTo("Street with id 2/2 not found."));
        StreetRepository.Verify(x => x.GetByIdAsync(2, 2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        StreetRepository.Setup(x => x.ExistAnyAsync()).ReturnsAsync(true);

        //Act
        var result = await StreetService.ExistAnyAsync();

        //Assert
        StreetRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}