using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Mappers;

namespace TerrytLookup.Tests.MappersTests.VoivodeshipsTests;

public class VoivodeshipMappersTests
{
    private readonly VerifySettings _settings;

    public VoivodeshipMappersTests()
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
            Name = "Mazowieckie",
            EntityType = "województwo",
            ValidFromDate = new DateOnly(2025, 1, 1)
        };

        //Act
        var result = source.ToDomainVoivodeship();

        //Assert
        return Verify(result, _settings);
    }

    [Test]
    public Task ShouldMapToDto()
    {
        //Arrange
        var source = new Voivodeship
        {
            Id = 14,
            Name = "Mazowieckie",
            ValidFromDate = new DateOnly(2025, 1, 1),
            CreateTimestamp = new DateTime(2025, 1, 2),
            UpdateTimestamp = new DateTime(2025, 1, 3)
        };

        //Act
        var result = source.ToDto();

        //Assert
        return Verify(result, _settings);
    }
}