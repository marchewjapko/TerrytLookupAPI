using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.CountyService;
using TerrytLookup.WebAPI.Controllers;

namespace TerrytLookup.Tests.ControllerTests;

public class CountyControllerTests
{
    private static readonly Mock<ICountyService> CountyService = new();

    private static readonly CountyController CountyController = new(CountyService.Object);

    [Test]
    public void BrowseAllCounties_ShouldReturnAllCounties()
    {
        // Arrange
        var expectedResult = Builder<CountyDto>.CreateListOfSize(10)
            .Build();

        CountyService.Setup(x => x.BrowseAllAsync(null, null))
            .Returns(expectedResult);

        // Act
        var result = CountyController.BrowseAllCounties() as OkObjectResult;

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<List<CountyDto>>());
        });
    }

    [Test]
    public async Task GetCountyById_ShouldReturnCounty()
    {
        // Arrange
        var expectedResult = Builder<CountyDto>.CreateNew()
            .Build();

        CountyService.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await CountyController.GetCountyById(1, 1) as OkObjectResult;

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<CountyDto>());
        });
    }
}