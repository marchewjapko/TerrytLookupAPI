using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Tests.MappersTests.TownTests;

public class TownMappersTests
{
    private readonly VerifySettings _settings;

    public TownMappersTests()
    {
        _settings = new VerifySettings();
        _settings.DontScrubDateTimes();
        _settings.ScrubMember<BaseEntity>(x => x.CreateTimestamp);
    }

    [Test]
    public Task ShouldMapToDomain()
    {
        //Arrange
        var source = new SimcDto
        {
            VoivodeshipId = 14,
            CountyId = 65,
            Name = "Warszawa",
            Id = 918123,
            ParentId = 918123,
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
        var source = new Town
        {
            Id = 918123,
            Name = "Warszawa",
            ParentTownId = null,
            ParentTown = null,
            CountyId = 65,
            CountyVoivodeshipId = 14,
            County = new County
            {
                VoivodeshipId = 14,
                CountyId = 65,
                Name = "Warszawa",
                ValidFromDate = new DateOnly(2025, 1, 1),
                Voivodeship = new Voivodeship
                {
                    Id = 14,
                    Name = "Mazowieckie",
                    ValidFromDate = new DateOnly(2025, 1, 1)
                }
            }
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
        var voivodeship = new Voivodeship
        {
            Id = 14,
            Name = "Mazowieckie",
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        var county = new County
        {
            VoivodeshipId = 14,
            CountyId = 65,
            Name = "Warszawa",
            ValidFromDate = new DateOnly(2025, 1, 1),
            Voivodeship = voivodeship
        };

        var source = new Town
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
                County = county
            },
            CountyId = 65,
            CountyVoivodeshipId = 14,
            County = county
        };

        //Act
        var result = source.ToDto();

        //Assert
        return Verify(result, _settings);
    }
}