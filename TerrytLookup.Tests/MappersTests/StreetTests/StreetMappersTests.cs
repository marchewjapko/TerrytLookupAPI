using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Tests.MappersTests.StreetTests;

public class StreetMappersTests
{
    private static readonly Voivodeship Voivodeship = new()
    {
        Id = 14,
        Name = "Mazowieckie",
        ValidFromDate = new DateOnly(2025, 1, 1)
    };

    private static readonly County County = new()
    {
        VoivodeshipId = 14,
        CountyId = 65,
        Name = "Warszawa",
        ValidFromDate = new DateOnly(2025, 1, 1),
        Voivodeship = Voivodeship
    };

    private static readonly Town Town = new()
    {
        Id = 918130,
        Name = "Mokotów",
        ParentTownId = null,
        ParentTown = new Town
        {
            Id = 918123,
            Name = "Warszawa",
            ParentTownId = null,
            ParentTown = null,
            CountyId = 65,
            CountyVoivodeshipId = 14,
            County = County
        },
        CountyId = 65,
        CountyVoivodeshipId = 14,
        County = County
    };

    private readonly VerifySettings _settings;

    public StreetMappersTests()
    {
        _settings = new VerifySettings();
        _settings.DontScrubDateTimes();
        _settings.ScrubMember<BaseEntity>(x => x.CreateTimestamp);
    }

    [Test]
    public Task ShouldMapToDomain()
    {
        //Arrange
        var source = new UlicDto
        {
            TownId = Town.Id,
            StreetNameId = 21435,
            StreetPrefix = "ul.",
            StreetNameFirstPart = "Stwosza",
            StreetNameSecondPart = "Wita",
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        //Act
        var result = source.ToDomain();

        //Assert
        return Verify(result, _settings);
    }

    [Test]
    public Task ShouldMapToDto_NoParent()
    {
        //Arrange
        var town = new Town
        {
            Id = 918123,
            Name = "Warszawa",
            ParentTownId = null,
            ParentTown = null,
            CountyId = 65,
            CountyVoivodeshipId = 14,
            County = County
        };

        var source = new Street
        {
            NameId = 21435,
            Name = "ul. Wita Stwosza",
            Town = town,
            TownId = town.Id,
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        //Act
        var result = source.ToDto();

        //Assert
        return Verify(result, _settings);
    }

    [Test]
    public Task ShouldMapToDto_WithParent()
    {
        //Arrange
        var source = new Street
        {
            NameId = 21435,
            Name = "ul. Wita Stwosza",
            Town = Town,
            TownId = Town.Id,
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        //Act
        var result = source.ToDto();

        //Assert
        return Verify(result, _settings);
    }
}