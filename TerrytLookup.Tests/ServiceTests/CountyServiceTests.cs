using Bogus;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.CountyService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class CountyServiceTests
{
    private static readonly Mock<ICountyRepository> CountyRepository = new();
    private static readonly CountyService CountyService = new(CountyRepository.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var tercDtos = Builder<TercDto>.CreateListOfSize(10).Build();

        //Act
        await CountyService.AddRange(tercDtos);

        //Assert
        CountyRepository.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<County>>()), Times.Once);
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnAllCounties()
    {
        //Arrange
        const int countiesSize = 10;
        var voivodeship = Builder<Voivodeship>.CreateNew().Build();

        var counties = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate(countiesSize)
            .ToAsyncEnumerable();

        CountyRepository.Setup(x => x.BrowseAllAsync(null, null)).Returns(counties);

        //Act
        var result = await CountyService.BrowseAllAsync().ToListAsync();

        //Assert
        CountyRepository.Verify(x => x.BrowseAllAsync(null, null), Times.Once);
        Assert.That(result, Has.Count.EqualTo(countiesSize));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCounty()
    {
        //Arrange
        var voivodeship = Builder<Voivodeship>.CreateNew().Build();

        var county = new Faker<County>().RuleFor(x => x.Voivodeship, _ => voivodeship).Generate();

        CountyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(county);

        //Act
        var result = await CountyService.GetByIdAsync(1, 1);

        //Assert
        CountyRepository.Verify(x => x.GetByIdAsync(1, 1), Times.Once);
        Assert.That(result.CountyId, Is.EqualTo(county.CountyId));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowCountyNotFoundException()
    {
        //Arrange
        CountyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((County?)null);

        //Assert
        var exception = Assert.ThrowsAsync<CountyNotFoundException>(() => CountyService.GetByIdAsync(2, 2));

        Assert.That(exception.Message, Is.EqualTo("County with id 2/2 not found."));
        CountyRepository.Verify(x => x.GetByIdAsync(2, 2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        CountyRepository.Setup(x => x.ExistAnyAsync()).ReturnsAsync(true);

        //Act
        var result = await CountyService.ExistAnyAsync();

        //Assert
        CountyRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}