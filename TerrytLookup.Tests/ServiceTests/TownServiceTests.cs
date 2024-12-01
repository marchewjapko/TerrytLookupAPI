using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Services.TownService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class TownServiceTests
{
    private static readonly Mock<ITownRepository> TownRepository = new();
    private static readonly Mock<IMapper> Mapper = new();
    private static readonly TownService TownService = new(TownRepository.Object, Mapper.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .Build();
        var townDtos = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<Town>>(It.IsAny<IEnumerable<CreateTownDto>>()))
            .Returns(towns);

        //Act
        await TownService.AddRange(townDtos);

        //Assert
        TownRepository.Verify(x => x.AddRangeAsync(towns), Times.Once);
    }

    [Test]
    public void BrowseAllAsync_ShouldReturnAllTowns()
    {
        //Arrange
        var towns = Builder<Town>.CreateListOfSize(10)
            .Build()
            .ToAsyncEnumerable();

        var townDtos = Builder<TownDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<TownDto>>(It.IsAny<IAsyncEnumerable<Town>>()))
            .Returns(townDtos);

        TownRepository.Setup(x => x.BrowseAllAsync(null, null, null))
            .Returns(towns);

        //Act
        var result = TownService.BrowseAllAsync();

        //Assert
        TownRepository.Verify(x => x.BrowseAllAsync(null, null, null), Times.Once);
        Assert.That(result, Is.EquivalentTo(townDtos));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnTown()
    {
        //Arrange
        var town = Builder<Town>.CreateNew()
            .Build();

        var townDto = Builder<TownDto>.CreateNew()
            .Build();

        TownRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(town);

        Mapper.Setup(x => x.Map<TownDto>(It.IsAny<Town>()))
            .Returns(townDto);

        //Act
        var result = await TownService.GetByIdAsync(1);

        //Assert
        TownRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
        Assert.That(result, Is.EqualTo(townDto));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowTownNotFoundException()
    {
        //Arrange
        TownRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Town?)null);

        //Assert
        var exception = Assert.ThrowsAsync<TownNotFoundException>(
            () => TownService.GetByIdAsync(2));

        Assert.That(exception.Message, Is.EqualTo("Town with id 2 not found."));
        TownRepository.Verify(x => x.GetByIdAsync(2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        TownRepository.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        //Act
        var result = await TownService.ExistAnyAsync();

        //Assert
        TownRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}