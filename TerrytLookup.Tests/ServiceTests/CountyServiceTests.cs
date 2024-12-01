using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Services.CountyService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class CountyServiceTests
{
    private static readonly Mock<ICountyRepository> CountyRepository = new();
    private static readonly Mock<IMapper> Mapper = new();
    private static readonly CountyService CountyService = new(CountyRepository.Object, Mapper.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var counties = Builder<County>.CreateListOfSize(10)
            .Build();
        var countyDtos = Builder<CreateCountyDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<County>>(It.IsAny<IEnumerable<CreateCountyDto>>()))
            .Returns(counties);

        //Act
        await CountyService.AddRange(countyDtos);

        //Assert
        CountyRepository.Verify(x => x.AddRangeAsync(counties), Times.Once);
    }

    [Test]
    public void BrowseAllAsync_ShouldReturnAllCounties()
    {
        //Arrange
        var counties = Builder<County>.CreateListOfSize(10)
            .Build()
            .ToAsyncEnumerable();

        var countyDtos = Builder<CountyDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<CountyDto>>(It.IsAny<IAsyncEnumerable<County>>()))
            .Returns(countyDtos);

        CountyRepository.Setup(x => x.BrowseAllAsync(null, null))
            .Returns(counties);

        //Act
        var result = CountyService.BrowseAllAsync();

        //Assert
        CountyRepository.Verify(x => x.BrowseAllAsync(null, null), Times.Once);
        Assert.That(result, Is.EquivalentTo(countyDtos));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCounty()
    {
        //Arrange
        var county = Builder<County>.CreateNew()
            .Build();

        var countyDto = Builder<CountyDto>.CreateNew()
            .Build();

        CountyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(county);

        Mapper.Setup(x => x.Map<CountyDto>(It.IsAny<County>()))
            .Returns(countyDto);

        //Act
        var result = await CountyService.GetByIdAsync(1, 1);

        //Assert
        CountyRepository.Verify(x => x.GetByIdAsync(1, 1), Times.Once);
        Assert.That(result, Is.EqualTo(countyDto));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowCountyNotFoundException()
    {
        //Arrange
        CountyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((County?)null);

        //Assert
        var exception = Assert.ThrowsAsync<CountyNotFoundException>(
            () => CountyService.GetByIdAsync(2, 2));

        Assert.That(exception.Message, Is.EqualTo("County with id 2/2 not found."));
        CountyRepository.Verify(x => x.GetByIdAsync(2, 2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        CountyRepository.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        //Act
        var result = await CountyService.ExistAnyAsync();

        //Assert
        CountyRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}