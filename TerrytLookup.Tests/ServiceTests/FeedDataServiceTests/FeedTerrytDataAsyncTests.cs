using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Moq;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.CountyService;
using TerrytLookup.Infrastructure.Services.FeedDataService;
using TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;
using TerrytLookup.Infrastructure.Services.StreetService;
using TerrytLookup.Infrastructure.Services.TownService;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;

namespace TerrytLookup.Tests.ServiceTests.FeedDataServiceTests;

public class FeedTerrytDataAsyncTests
{
    private static Mock<IVoivodeshipService> _voivodeshipService = new();
    private static Mock<ICountyService> _countyService = new();
    private static Mock<ITownService> _townService = new();
    private static Mock<IStreetService> _streetService = new();
    private static Mock<ITerrytReader> _terrytReader = new();

    private static FeedDataService _feedDataService = new(_voivodeshipService.Object, _countyService.Object,
        _townService.Object, _streetService.Object, _terrytReader.Object);

    [SetUp]
    public void Setup()
    {
        _voivodeshipService = new Mock<IVoivodeshipService>();
        _countyService = new Mock<ICountyService>();
        _townService = new Mock<ITownService>();
        _streetService = new Mock<IStreetService>();
        _terrytReader = new Mock<ITerrytReader>();
        _feedDataService = new FeedDataService(_voivodeshipService.Object, _countyService.Object, _townService.Object,
            _streetService.Object, _terrytReader.Object);
    }

    [Test]
    [NonParallelizable]
    public async Task FeedTerrytDataAsync_ShouldReadData()
    {
        //Arrange
        var tercSet = Builder<TercDto>.CreateListOfSize(10).Build().ToList();

        var simcSet = Builder<SimcDto>.CreateListOfSize(10).Build().ToList();

        var ulicSet = Builder<UlicDto>.CreateListOfSize(10).Build().ToList();

        _terrytReader.Setup(x => x.ReadAsync<TercDto>(It.IsAny<IFormFile>())).ReturnsAsync(tercSet);
        _terrytReader.Setup(x => x.ReadAsync<SimcDto>(It.IsAny<IFormFile>())).ReturnsAsync(simcSet);
        _terrytReader.Setup(x => x.ReadAsync<UlicDto>(It.IsAny<IFormFile>())).ReturnsAsync(ulicSet);

        //Act
        await _feedDataService.FeedTerrytDataAsync(new Mock<IFormFile>().Object, new Mock<IFormFile>().Object,
            new Mock<IFormFile>().Object);

        //Assert
        _voivodeshipService.Verify(x => x.AddRange(It.IsAny<IEnumerable<TercDto>>()), Times.Once);
        _countyService.Verify(x => x.AddRange(It.IsAny<IEnumerable<TercDto>>()), Times.Once);
        _townService.Verify(x => x.AddRange(It.IsAny<IEnumerable<SimcDto>>()), Times.Once);
        _streetService.Verify(x => x.AddRange(It.IsAny<IEnumerable<UlicDto>>()), Times.Once);
    }

    [Test]
    [NonParallelizable]
    public void FeedTerrytDataAsync_ShouldThrowTerrytParsingException()
    {
        //Arrange
        _terrytReader.Setup(x => x.ReadAsync<TercDto>(It.IsAny<IFormFile>()))
            .ThrowsAsync(new Exception("TestException"));

        //Assert
        var exception = Assert.ThrowsAsync<TerrytParsingException>(() =>
            _feedDataService.FeedTerrytDataAsync(new Mock<IFormFile>().Object, new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object));

        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Is.EqualTo("Unable to read file containing Terryt datasets."));
            Assert.That(exception.InnerException!.Message, Is.EqualTo("TestException"));
        });

        _voivodeshipService.Verify(x => x.AddRange(It.IsAny<IEnumerable<TercDto>>()), Times.Never);
        _countyService.Verify(x => x.AddRange(It.IsAny<IEnumerable<TercDto>>()), Times.Never);
        _townService.Verify(x => x.AddRange(It.IsAny<IEnumerable<SimcDto>>()), Times.Never);
        _streetService.Verify(x => x.AddRange(It.IsAny<IEnumerable<UlicDto>>()), Times.Never);
    }
}