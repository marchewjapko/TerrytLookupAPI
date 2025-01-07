using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.StreetService;
using TerrytLookup.WebAPI.Controllers;

namespace TerrytLookup.Tests.ControllerTests;

public class StreetControllerTests
{
    private static readonly Mock<IStreetService> StreetService = new();

    private static readonly StreetController StreetController = new(StreetService.Object);

    [Test]
    public void BrowseAllStreets_ShouldReturnAllStreets()
    {
        // Arrange
        var expectedResult = Builder<StreetDto>.CreateListOfSize(10).Build().ToAsyncEnumerable();

        StreetService.Setup(x => x.BrowseAllAsync(null, null)).Returns(expectedResult);

        // Act
        var result = StreetController.BrowseAllStreets() as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            // Assert.That(result.Value, Is.TypeOf<IAsyncEnumerable<StreetDto>>());
        });
    }

    [Test]
    public async Task GetStreetById_ShouldReturnStreet()
    {
        // Arrange
        var expectedResult = Builder<StreetDto>.CreateNew().Build();

        StreetService.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedResult);

        // Act
        var result = await StreetController.GetStreetById(1, 1) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<StreetDto>());
        });
    }
}