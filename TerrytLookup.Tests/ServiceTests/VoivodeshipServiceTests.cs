using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class VoivodeshipServiceTests
{
    private static readonly Mock<IVoivodeshipRepository> VoivodeshipRepository = new();
    private static readonly Mock<IMapper> Mapper = new();
    private static readonly VoivodeshipService VoivodeshipService = new(VoivodeshipRepository.Object, Mapper.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build();
        var voivodeshipsDto = Builder<CreateVoivodeshipDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<Voivodeship>>(It.IsAny<IEnumerable<CreateVoivodeshipDto>>()))
            .Returns(voivodeships);

        //Act
        await VoivodeshipService.AddRange(voivodeshipsDto);

        //Assert
        VoivodeshipRepository.Verify(x => x.AddRangeAsync(voivodeships), Times.Once);
    }

    [Test]
    public void BrowseAllAsync_ShouldReturnAllVoivodeships()
    {
        //Arrange
        var voivodeships = Builder<Voivodeship>.CreateListOfSize(10)
            .Build()
            .ToAsyncEnumerable();

        var voivodeshipsDto = Builder<VoivodeshipDto>.CreateListOfSize(10)
            .Build();

        Mapper.Setup(x => x.Map<IEnumerable<VoivodeshipDto>>(It.IsAny<IAsyncEnumerable<Voivodeship>>()))
            .Returns(voivodeshipsDto);

        VoivodeshipRepository.Setup(x => x.BrowseAllAsync())
            .Returns(voivodeships);

        //Act
        var result = VoivodeshipService.BrowseAllAsync();

        //Assert
        VoivodeshipRepository.Verify(x => x.BrowseAllAsync(), Times.Once);
        Assert.That(result, Is.EquivalentTo(voivodeshipsDto));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnVoivodeship()
    {
        //Arrange
        var voivodeship = Builder<Voivodeship>.CreateNew()
            .Build();

        var voivodeshipDto = Builder<VoivodeshipDto>.CreateNew()
            .Build();

        VoivodeshipRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(voivodeship);

        Mapper.Setup(x => x.Map<VoivodeshipDto>(It.IsAny<Voivodeship>()))
            .Returns(voivodeshipDto);

        //Act
        var result = await VoivodeshipService.GetByIdAsync(1);

        //Assert
        VoivodeshipRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
        Assert.That(result, Is.EqualTo(voivodeshipDto));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowVoivodeshipNotFoundException()
    {
        //Arrange
        VoivodeshipRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Voivodeship?)null);

        //Assert
        var exception = Assert.ThrowsAsync<VoivodeshipNotFoundException>(
            () => VoivodeshipService.GetByIdAsync(2));

        Assert.That(exception.Message, Is.EqualTo("Voivodeship with id 2 not found."));
        VoivodeshipRepository.Verify(x => x.GetByIdAsync(2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        VoivodeshipRepository.Setup(x => x.ExistAnyAsync())
            .ReturnsAsync(true);

        //Act
        var result = await VoivodeshipService.ExistAnyAsync();

        //Assert
        VoivodeshipRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}