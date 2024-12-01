using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.CountyService;
using TerrytLookup.Infrastructure.Services.FeedDataService;
using TerrytLookup.Infrastructure.Services.StreetService;
using TerrytLookup.Infrastructure.Services.TownService;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;
using TerrytLookup.WebAPI.Controllers;

namespace TerrytLookup.Tests.ControllerTests;

public class DataFeedControllerTests
{
    private static Mock<ITownService> _townService = new();
    private static  Mock<IStreetService> _streetService = new();
    private static  Mock<IVoivodeshipService> _voivodeshipService = new();
    private static  Mock<ICountyService> _countyService = new();
    private static  Mock<IFeedDataService> _feedDataService = new();
    private static  Mock<ILogger<DataFeedController>> _logger = new();

    private static DataFeedController _dataFeedController = new(_logger.Object,
        _townService.Object,
        _streetService.Object,
        _voivodeshipService.Object,
        _countyService.Object,
        _feedDataService.Object);

    [SetUp]
    public void Setup()
    {
        _townService = new Mock<ITownService>();
        _streetService = new Mock<IStreetService>();
        _voivodeshipService = new Mock<IVoivodeshipService>();
        _countyService = new Mock<ICountyService>();
        _feedDataService = new Mock<IFeedDataService>();
        _logger = new Mock<ILogger<DataFeedController>>();
        _dataFeedController = new(_logger.Object,
            _townService.Object,
            _streetService.Object,
            _voivodeshipService.Object,
            _countyService.Object,
            _feedDataService.Object);
    }
    
    [Test]
    public async Task InitializeData_ShouldInitialize()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();

        tercFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");

        _townService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _streetService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _countyService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _voivodeshipService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);

        // Act
        var result = await _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object) as NoContentResult;

        // Assert
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Once);

        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(204));
        });
    }

    [Test]
    public void InitializeData_ShouldThrowInvalidFileContentTypeExtensionException()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();

        tercFile.Setup(x => x.ContentType)
            .Returns("text/txt");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        
        // Assert
        var exception = Assert.ThrowsAsync<InvalidFileContentTypeExtensionException>(
            () => _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object));
        
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Never);
        Assert.That(exception.Message, Is.EqualTo("Unable to read file with content type of 'text/txt'."));
    }
    
    [Test]
    public void InitializeData_ShouldThrowDatabaseNotEmptyException_Towns()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();

        tercFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");

        _townService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        // Assert
        var exception = Assert.ThrowsAsync<DatabaseNotEmptyException>(
            () => _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object));
        
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Never);
        Assert.That(exception.Message, Is.EqualTo("Unable to initialize registers. The database must be empty."));
    }
    
    [Test]
    public void InitializeData_ShouldThrowDatabaseNotEmptyException_Streets()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();
        
        tercFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");

        _townService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _streetService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        // Assert
        var exception = Assert.ThrowsAsync<DatabaseNotEmptyException>(
            () => _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object));
        
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Never);
        Assert.That(exception.Message, Is.EqualTo("Unable to initialize registers. The database must be empty."));
    }
    
    [Test]
    public void InitializeData_ShouldThrowDatabaseNotEmptyException_Voivodeships()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();
        
        tercFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");

        _townService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _streetService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _voivodeshipService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        // Assert
        var exception = Assert.ThrowsAsync<DatabaseNotEmptyException>(
            () => _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object));
        
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Never);
        Assert.That(exception.Message, Is.EqualTo("Unable to initialize registers. The database must be empty."));
    }
    
    [Test]
    public void InitializeData_ShouldThrowDatabaseNotEmptyException_Counties()
    {
        // Arrange
        var tercFile = new Mock<IFormFile>();
        var simcFile = new Mock<IFormFile>();
        var ulicFile = new Mock<IFormFile>();

        tercFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        simcFile.Setup(x => x.ContentType)
            .Returns("text/csv");
        ulicFile.Setup(x => x.ContentType)
            .Returns("text/csv");

        _townService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _streetService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _voivodeshipService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(false);
        _countyService.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        // Assert
        var exception = Assert.ThrowsAsync<DatabaseNotEmptyException>(
            () => _dataFeedController.InitializeData(tercFile.Object, simcFile.Object, ulicFile.Object));
        
        _feedDataService.Verify(x => x.FeedTerrytDataAsync(It.IsAny<IFormFile>(), It.IsAny<IFormFile>(), It.IsAny<IFormFile>()), Times.Never);
        Assert.That(exception.Message, Is.EqualTo("Unable to initialize registers. The database must be empty."));
    }
}