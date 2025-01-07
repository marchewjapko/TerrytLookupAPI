using FizzWare.NBuilder;
using Moq;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;

namespace TerrytLookup.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class VoivodeshipServiceTests
{
    private static readonly Mock<IVoivodeshipRepository> VoivodeshipRepository = new();
    private static readonly VoivodeshipService VoivodeshipService = new(VoivodeshipRepository.Object);

    [Test]
    public async Task AddRange_ShouldAddRange()
    {
        //Arrange
        var tercDtos = Builder<TercDto>.CreateListOfSize(10).Build();

        //Act
        await VoivodeshipService.AddRange(tercDtos);

        //Assert
        VoivodeshipRepository.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<Voivodeship>>()), Times.Once);
    }

    [Test]
    public async Task BrowseAllAsync_ShouldReturnAllVoivodeships()
    {
        //Arrange
        const int voivodeshipSize = 10;

        var voivodeships = Builder<Voivodeship>.CreateListOfSize(voivodeshipSize).Build().ToAsyncEnumerable();

        VoivodeshipRepository.Setup(x => x.BrowseAllAsync()).Returns(voivodeships);

        //Act
        var result = await VoivodeshipService.BrowseAllAsync().ToListAsync();

        //Assert
        VoivodeshipRepository.Verify(x => x.BrowseAllAsync(), Times.Once);
        Assert.That(result, Has.Count.EqualTo(voivodeshipSize));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnVoivodeship()
    {
        //Arrange
        var voivodeship = Builder<Voivodeship>.CreateNew().Build();

        VoivodeshipRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(voivodeship);

        //Act
        var result = await VoivodeshipService.GetByIdAsync(1);

        //Assert
        VoivodeshipRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
        Assert.That(result.Id, Is.EqualTo(voivodeship.Id));
    }

    [Test]
    public void GetByIdAsync_ShouldThrowVoivodeshipNotFoundException()
    {
        //Arrange
        VoivodeshipRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Voivodeship?)null);

        //Assert
        var exception = Assert.ThrowsAsync<VoivodeshipNotFoundException>(() => VoivodeshipService.GetByIdAsync(2));

        Assert.That(exception.Message, Is.EqualTo("Voivodeship with id 2 not found."));
        VoivodeshipRepository.Verify(x => x.GetByIdAsync(2), Times.Once);
    }

    [Test]
    public async Task ExistAnyAsync_ShouldReturnResult()
    {
        //Arrange
        VoivodeshipRepository.Setup(x => x.ExistAnyAsync()).ReturnsAsync(true);

        //Act
        var result = await VoivodeshipService.ExistAnyAsync();

        //Assert
        VoivodeshipRepository.Verify(x => x.ExistAnyAsync(), Times.Once);
        Assert.That(result, Is.True);
    }
}