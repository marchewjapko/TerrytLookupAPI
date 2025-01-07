using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.VoivodeshipService;
using TerrytLookup.WebAPI.Controllers;

namespace TerrytLookup.Tests.ControllerTests;

public class VoivodeshipControllerTests
{
    private static readonly Mock<IVoivodeshipService> VoivodeshipService = new();

    private static readonly VoivodeshipController VoivodeshipController = new(VoivodeshipService.Object);

    [Test]
    public void BrowseAllVoivodeships_ShouldReturnAllVoivodeships()
    {
        // Arrange
        var expectedResult = Builder<VoivodeshipDto>.CreateListOfSize(10).Build().ToAsyncEnumerable();

        VoivodeshipService.Setup(x => x.BrowseAllAsync()).Returns(expectedResult);

        // Act
        var result = VoivodeshipController.BrowseAllVoivodeships() as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            // Assert.That(result.Value, Is.TypeOf<List<VoivodeshipDto>>());
        });
    }

    [Test]
    public async Task GetVoivodeshipById_ShouldReturnVoivodeship()
    {
        // Arrange
        var expectedResult = Builder<VoivodeshipDto>.CreateNew().Build();

        VoivodeshipService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedResult);

        // Act
        var result = await VoivodeshipController.GetVoivodeshipById(1) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<VoivodeshipDto>());
        });
    }
}