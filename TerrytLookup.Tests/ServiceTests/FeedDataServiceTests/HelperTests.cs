using Bogus;
using FizzWare.NBuilder;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Services.FeedDataService;

namespace TerrytLookup.Tests.ServiceTests.FeedDataServiceTests;

public class HelperTests
{
    [Test]
    public void AssignParentToTown_ShouldAssign()
    {
        //Arrange
        var allTowns = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchTown = new CreateTownDto
        {
            Name = new Faker().Address.City(),
            TerrytId = allTowns.First()
                .Key,
            ParentTownTerrytId = allTowns.Last()
                .Key,
            CountyTerrytId = (1, 1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchTown.ParentTown, Is.Null);

        //Act
        FeedDataService.AssignParentToTown(searchTown, allTowns);

        //Assert
        Assert.Multiple(() => {
            Assert.That(searchTown.ParentTown, Is.Not.Null);
            Assert.That(searchTown.ParentTown!.TerrytId, Is.EqualTo(allTowns.Last()
                .Value.TerrytId));
        });
    }

    [Test]
    public void AssignParentToTown_ShouldSkipWhenItsOwnParent()
    {
        //Arrange
        var allTowns = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchTown = new CreateTownDto
        {
            Name = new Faker().Address.City(),
            TerrytId = 1,
            ParentTownTerrytId = 1,
            CountyTerrytId = (1, 1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchTown.ParentTown, Is.Null);

        //Act
        FeedDataService.AssignParentToTown(searchTown, allTowns);

        //Assert
        Assert.That(searchTown.ParentTown, Is.Null);
    }

    [Test]
    public void AssignParentToTown_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var allTowns = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchTown = new CreateTownDto
        {
            Name = new Faker().Address.City(),
            TerrytId = 1,
            ParentTownTerrytId = allTowns.Max(x => x.Key) + 1,
            CountyTerrytId = (1, 1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchTown.ParentTown, Is.Null);

        //Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => FeedDataService.AssignParentToTown(searchTown, allTowns));

        Assert.Multiple(() => {
            Assert.That(searchTown.ParentTown, Is.Null);
            Assert.That(exception.Message, Is.EqualTo($"Town {searchTown.Name} does not have a parent town."));
        });
    }

    [Test]
    public void AssignTownToCounty_ShouldAssign()
    {
        //Arrange
        var counties = new Faker<CreateCountyDto>()
            .RuleFor(x => x.TerrytId,
                f => (f.IndexFaker,
                    f.IndexFaker)) //Can't use NBuilder for this one, since "x.TerrytId" is an init-only property. Faker doesn't seem to mind
            .Generate(5)
            .ToDictionary(x => x.TerrytId, x => x);

        var searchTown = new CreateTownDto
        {
            Name = new Faker().Address.City(),
            TerrytId = 1,
            ParentTownTerrytId = 1,
            CountyTerrytId = counties.First()
                .Key,
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchTown.County, Is.Null);

        //Act
        FeedDataService.AssignTownToCounty(searchTown, counties);

        //Assert
        Assert.Multiple(() => {
            Assert.That(searchTown.County, Is.Not.Null);
            Assert.That(searchTown.County!.TerrytId, Is.EqualTo(counties.First()
                .Value.TerrytId));
        });
    }

    [Test]
    public void AssignTownToCounty_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var counties = new Faker<CreateCountyDto>()
            .RuleFor(x => x.TerrytId,
                f => (f.IndexFaker,
                    f.IndexFaker)) //Can't use NBuilder for this one, since "x.TerrytId" is an init-only property. Faker doesn't seem to mind
            .Generate(5)
            .ToDictionary(x => x.TerrytId, x => x);

        var searchTown = new CreateTownDto
        {
            Name = new Faker().Address.City(),
            TerrytId = 1,
            ParentTownTerrytId = 1,
            CountyTerrytId = (-1, -1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchTown.County, Is.Null);

        //Act
        //Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => FeedDataService.AssignTownToCounty(searchTown, counties));

        Assert.Multiple(() => {
            Assert.That(searchTown.County, Is.Null);
            Assert.That(exception.Message, Is.EqualTo($"Town {searchTown.Name} is not a part of any county."));
        });
    }

    [Test]
    public void AssignCountyToVoivodeship_ShouldAssign()
    {
        //Arrange
        var voivodeships = Builder<CreateVoivodeshipDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchCounty = new CreateCountyDto
        {
            Name = new Faker().Address.City(),
            TerrytId = (voivodeships.First()
                .Key, 1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchCounty.Voivodeship, Is.Null);

        //Act
        FeedDataService.AssignCountyToVoivodeship(searchCounty, voivodeships);

        //Assert
        Assert.Multiple(() => {
            Assert.That(searchCounty.Voivodeship, Is.Not.Null);
            Assert.That(searchCounty.Voivodeship!.TerrytId, Is.EqualTo(voivodeships.First()
                .Value.TerrytId));
        });
    }

    [Test]
    public void AssignCountyToVoivodeship_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var voivodeships = Builder<CreateVoivodeshipDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchCounty = new CreateCountyDto
        {
            Name = new Faker().Address.City(),
            TerrytId = (voivodeships.Max(x => x.Key) + 1, 1),
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchCounty.Voivodeship, Is.Null);

        //Act
        //Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => FeedDataService.AssignCountyToVoivodeship(searchCounty, voivodeships));

        Assert.Multiple(() => {
            Assert.That(searchCounty.Voivodeship, Is.Null);
            Assert.That(exception.Message, Is.EqualTo($"County {searchCounty.Name} is not a part of any voivodeship."));
        });
    }

    [Test]
    public void AssignStreetToTown_ShouldAssign()
    {
        //Arrange
        var towns = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchStreet = new CreateStreetDto
        {
            Name = new Faker().Address.City(),
            TerrytTownId = towns.First()
                .Key,
            TerrytNameId = 1,
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchStreet.Town, Is.Null);

        //Act
        FeedDataService.AssignStreetToTown(searchStreet, towns);

        //Assert
        Assert.Multiple(() => {
            Assert.That(searchStreet.Town, Is.Not.Null);
            Assert.That(searchStreet.Town!.TerrytId, Is.EqualTo(towns.First()
                .Value.TerrytId));
        });
    }

    [Test]
    public void AssignStreetToTown_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var towns = Builder<CreateTownDto>.CreateListOfSize(10)
            .Build()
            .ToDictionary(x => x.TerrytId, x => x);

        var searchStreet = new CreateStreetDto
        {
            Name = new Faker().Address.City(),
            TerrytTownId = towns.Max(x => x.Key) + 1,
            TerrytNameId = 1,
            ValidFromDate = new DateOnly()
        };

        Assume.That(searchStreet.Town, Is.Null);

        //Act
        //Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => FeedDataService.AssignStreetToTown(searchStreet, towns));

        Assert.Multiple(() => {
            Assert.That(searchStreet.Town, Is.Null);
            Assert.That(exception.Message, Is.EqualTo($"Street {searchStreet.Name} is not a part of any town."));
        });
    }
}