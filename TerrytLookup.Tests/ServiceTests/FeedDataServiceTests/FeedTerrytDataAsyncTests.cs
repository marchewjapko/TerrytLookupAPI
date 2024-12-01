using AutoMapper;
using Bogus;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Moq;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.FeedDataService;
using TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;

namespace TerrytLookup.Tests.ServiceTests.FeedDataServiceTests;

public class FeedTerrytDataAsyncTests
{
    private static Mock<IMapper> _mapper = new();
    private static Mock<IVoivodeshipService> _voivodeshipService = new();
    private static Mock<ITerrytReader> _terrytReader = new();
    private static FeedDataService _feedDataService = new(_mapper.Object, _voivodeshipService.Object, _terrytReader.Object);

    [SetUp]
    public void Setup()
    {
        _mapper = new Mock<IMapper>();
        _voivodeshipService = new Mock<IVoivodeshipService>();
        _terrytReader = new Mock<ITerrytReader>();
        _feedDataService = new FeedDataService(_mapper.Object, _voivodeshipService.Object, _terrytReader.Object);
    }

    [Test]
    public async Task FeedTerrytDataAsync_ShouldReadData()
    {
        //Arrange
        var tercSet = Builder<TercDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();
        var simcSet = Builder<SimcDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();
        var ulicSet = Builder<UlicDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();

        _terrytReader.Setup(x => x.ReadAsync<TercDto>(It.IsAny<IFormFile>()))
            .ReturnsAsync(tercSet);
        _terrytReader.Setup(x => x.ReadAsync<SimcDto>(It.IsAny<IFormFile>()))
            .ReturnsAsync(simcSet);
        _terrytReader.Setup(x => x.ReadAsync<UlicDto>(It.IsAny<IFormFile>()))
            .ReturnsAsync(ulicSet);

        var voivodeshipDict = Builder<CreateVoivodeshipDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var countiesDict = new Faker<CreateCountyDto>().RuleFor(x => x.TerrytId, f => (f.Random.Int(1, voivodeshipDict.Count), f.IndexFaker))
            .Generate(20)
            .ToDictionary(x => x.TerrytId, x => x);

        var townsDict = new Faker<CreateTownDto>()
            .RuleFor(x => x.TerrytId, f => f.IndexFaker)
            .RuleFor(x => x.CountyTerrytId, _ => countiesDict.OrderBy(arg => Guid.NewGuid())
                .First()
                .Key)
            .Generate(30)
            .ToDictionary(x => x.TerrytId, x => x);

        var streetsList = new Faker<CreateStreetDto>()
            .RuleFor(x => x.TerrytTownId, f => f.Random.Int(0, townsDict.Count - 1))
            .Generate(40);

        _mapper.Setup(x => x.Map<Dictionary<int, CreateVoivodeshipDto>>(It.IsAny<IEnumerable<TercDto>>()))
            .Returns(voivodeshipDict);
        _mapper.Setup(x => x.Map<Dictionary<(int voivodeshipId, int countyId), CreateCountyDto>>(It.IsAny<IEnumerable<TercDto>>()))
            .Returns(countiesDict);
        _mapper.Setup(x => x.Map<Dictionary<int, CreateTownDto>>(It.IsAny<HashSet<SimcDto>>()))
            .Returns(townsDict);
        _mapper.Setup(x => x.Map<IEnumerable<CreateStreetDto>>(It.IsAny<HashSet<UlicDto>>()))
            .Returns(streetsList);

        //Act
        await _feedDataService.FeedTerrytDataAsync(new Mock<IFormFile>().Object, new Mock<IFormFile>().Object, new Mock<IFormFile>().Object);

        //Assert
        _voivodeshipService.Verify(x => x.AddRange(It.IsAny<IEnumerable<CreateVoivodeshipDto>>()), Times.Once);
    }

    [Test]
    public void FeedTerrytDataAsync_ShouldThrowTerrytParsingException()
    {
        //Arrange
        var tercSet = Builder<TercDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();
        var simcSet = Builder<SimcDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();
        var ulicSet = Builder<UlicDto>.CreateListOfSize(10)
            .Build()
            .ToHashSet();

        _terrytReader.Setup(x => x.ReadAsync<TercDto>(It.IsAny<IFormFile>()))
            .ThrowsAsync(new Exception("TestException"));

        //Assert
        var exception = Assert.ThrowsAsync<TerrytParsingException>(
            () => _feedDataService.FeedTerrytDataAsync(new Mock<IFormFile>().Object, new Mock<IFormFile>().Object, new Mock<IFormFile>().Object));
        Assert.Multiple(() => {
            Assert.That(exception.Message, Is.EqualTo("Unable to read file containing Terryt datasets."));
            Assert.That(exception.InnerException!.Message, Is.EqualTo("TestException"));
        });
        _voivodeshipService.Verify(x => x.AddRange(It.IsAny<IEnumerable<CreateVoivodeshipDto>>()), Times.Never);
    }
}