using Bogus;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Tests.DtoTests;

public class TercDtoTests
{
    [Test]
    public void IsVoivodeship_ShouldReturnTrue()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.EntityType, _ => "województwo")
            .Generate();

        //Act
        var result = tercDto.IsVoivodeship();

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsVoivodeship_ShouldReturnFalse()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.EntityType, _ => "county")
            .Generate();

        //Act
        var result = tercDto.IsVoivodeship();

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsCounty_ShouldReturnTrue()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.EntityType, _ => "powiat")
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
        var tercDto = new Faker<TercDto>().RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.EntityType, _ => "gmina")
            .Generate();

        //Act
        var result = tercDto.IsCounty();

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnTrue_SameRef()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.CountyId, _ => 1)
            .RuleFor(x => x.EntityType, _ => "gmina")
            .Generate();

        //Act
        var result = tercDto.Equals(tercDto);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnTrue_SameValue()
    {
        //Arrange
        var tercDto1 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        var tercDto2 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        //Act
        var result = tercDto1.Equals(tercDto2);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnFalse_Null()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        //Act
        var result = tercDto.Equals(null);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnFalse_DifferentValues()
    {
        //Arrange
        var tercDto1 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        var tercDto2 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 2)
            .RuleFor(x => x.CountyId, _ => 2)
            .Generate();

        //Act
        var result = tercDto1.Equals(tercDto2);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnFalse_Null()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        //Act
        var result = tercDto.Equals((object)null!);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnTrue_SameRef()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        //Act
        var result = tercDto.Equals((object)tercDto);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnFalse_DifferentType()
    {
        //Arrange
        var tercDto = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = tercDto.Equals(simcDto);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnTrue_SameValue()
    {
        //Arrange
        var tercDto1 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        var tercDto2 = new Faker<TercDto>().RuleFor(x => x.VoivodeshipId, _ => 1)
            .RuleFor(x => x.CountyId, _ => 1)
            .Generate();

        //Act
        var result = tercDto1.Equals((object)tercDto2);

        //Assert
        Assert.That(result, Is.True);
    }
}