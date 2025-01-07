using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Tests.MappersTests.CountyTests;

public class CountyMappersTests
{
    private readonly VerifySettings _settings;

    public CountyMappersTests()
    {
        _settings = new VerifySettings();
        _settings.DontScrubDateTimes();
        _settings.ScrubMember<BaseEntity>(x => x.CreateTimestamp);
    }

    [Test]
    public Task ShouldMapToDomain()
    {
        //Arrange
        var source = new TercDto
        {
            VoivodeshipId = 14,
            CountyId = 65,
            Name = "Warszawa",
            EntityType = "miasto stołeczne, na prawach powiatu",
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        //Act
        var result = source.ToDomainCounty();

        //Assert
        return Verify(result, _settings);
    }

    [Test]
    public Task ShouldMapToDto()
    {
        //Arrange
        var source = new County
        {
            VoivodeshipId = 14,
            CountyId = 65,
            Name = "Warszawa",
            ValidFromDate = new DateOnly(2025, 1, 1),
            Voivodeship = new Voivodeship
            {
                Id = 1,
                Name = "Mazowieckie",
                ValidFromDate = new DateOnly(2025, 1, 1)
            }
        };

        //Act
        var result = source.ToDto();

        //Assert
        return Verify(result, _settings);
    }
}