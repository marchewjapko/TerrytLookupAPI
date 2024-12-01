using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Services.StreetService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class StreetServiceTests
{
    private static readonly Mock<IStreetRepository> StreetRepository = new();
    private static readonly Mock<IMapper> Mapper = new();
    private static readonly StreetService StreetService = new(StreetRepository.Object, Mapper.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .Build();
        var streetDtos = Builder<CreateStreetDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<Street>>(It.IsAny<IEnumerable<CreateStreetDto>>()))
            .Returns(streets);

        //Act
        await StreetService.AddRange(streetDtos);

        //Assert
        StreetRepository.Verify(x => x.AddRangeAsync(streets), Times.Once);
    }

    [Test]
    public void BrowseAllAsync_ShouldReturnAllStreets()
    {
        //Arrange
        var streets = Builder<Street>.CreateListOfSize(10)
            .Build()
            .ToAsyncEnumerable();

        var streetDtos = Builder<StreetDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<StreetDto>>(It.IsAny<IAsyncEnumerable<Street>>()))
            .Returns(streetDtos);

        StreetRepository.Setup(x => x.BrowseAllAsync(null, null))
            .Returns(streets);

        //Act
        var result = StreetService.BrowseAllAsync();

        //Assert
        StreetRepository.Verify(x => x.BrowseAllAsync(null, null), Times.Once);
        Assert.That(result, Is.EquivalentTo(streetDtos));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnStreet()
    {
        //Arrange
        var street = Builder<Street>.CreateNew()
            .Build();

        var streetDto = Builder<StreetDto>.CreateNew()
            .Build();

        StreetRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(street);

        Mapper.Setup(x => x.Map<StreetDto>(It.IsAny<Street>()))
            .Returns(streetDto);

        //Act
        var result = await StreetService.GetByIdAsync(1, 1);

        //Assert
        StreetRepository.Verify(x => x.GetByIdAsync(1, 1), Times.Once);
        Assert.That(result, Is.EqualTo(streetDto));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowStreetNotFoundException()
    {
        //Arrange
        StreetRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((Street?)null);

        //Assert
        var exception = Assert.ThrowsAsync<StreetNotFoundException>(
            () => StreetService.GetByIdAsync(2, 2));

        Assert.That(exception.Message, Is.EqualTo("Street with id 2/2 not found."));
        StreetRepository.Verify(x => x.GetByIdAsync(2, 2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        StreetRepository.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        //Act
        var result = await StreetService.ExistAnyAsync();

        //Assert
        StreetRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}