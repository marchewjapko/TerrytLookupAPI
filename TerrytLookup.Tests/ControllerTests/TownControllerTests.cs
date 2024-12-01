using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.TownService;
using TerrytLookup.WebAPI.Controllers;

namespace TerrytLookup.Tests.ControllerTests;

public class TownControllerTests
{
    private static readonly Mock<ITownService> TownService = new();

    private static readonly TownController TownController = new(TownService.Object);

    [Test]
    public void BrowseAllTowns_ShouldReturnAllTowns()
    {
        // Arrange
        var expectedResult = Builder<TownDto>.CreateListOfSize(10)
            .Build();

        TownService.Setup(x => x.BrowseAllAsync(null, null, null))
            .Returns(expectedResult);

        // Act
        var result = TownController.BrowseAllTowns() as OkObjectResult;

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<List<TownDto>>());
        });
    }

    [Test]
    public async Task GetTownById_ShouldReturnTown()
    {
        // Arrange
        var expectedResult = Builder<TownDto>.CreateNew()
            .Build();

        TownService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await TownController.GetTownById(1) as OkObjectResult;

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<TownDto>());
        });
    }
}