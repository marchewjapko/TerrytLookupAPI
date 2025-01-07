using Bogus;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Tests.DtoTests;

public class TercDtoTests
{
    [Test]
    public void IsVoivodeship_ShouldReturnTrue()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.EntityType, _ => "województwo").Generate();

        //Act
        var result = tercDto.IsVoivodeship();

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsVoivodeship_ShouldReturnFalse()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.EntityType, _ => "county").Generate();

        //Act
        var result = tercDto.IsVoivodeship();

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsCounty_ShouldReturnTrue()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.CountyId, _ => 1).RuleFor(x => x.EntityType, _ => "powiat")
            .Generate();

        //Act
        var result = tercDto.IsCounty();

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsCounty_ShouldReturnFalse()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.CountyId, _ => 1).RuleFor(x => x.EntityType, _ => "gmina")
            .Generate();

        //Act
        var result = tercDto.IsCounty();

        //Assert
        Assert.That(result, Is.False);
    }
}