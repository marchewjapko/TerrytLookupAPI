using Bogus;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;

namespace TerrytLookup.Tests.DtoTests;

public class SimcDtoTests
{
    [Test]
    public void Equals_CompareToDto_ShouldReturnTrue_SameRef()
    {
        //Arrange
        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = simcDto.Equals(simcDto);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnTrue_SameValue()
    {
        //Arrange
        var simcDto1 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 1)
            .Generate();

        var simcDto2 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 1)
            .Generate();

        //Act
        var result = simcDto1.Equals(simcDto2);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnFalse_Null()
    {
        //Arrange
        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = simcDto.Equals(null);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToDto_ShouldReturnFalse_DifferentValues()
    {
        //Arrange
        var simcDto1 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 1)
            .Generate();

        var simcDto2 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 2)
            .Generate();

        //Act
        var result = simcDto1.Equals(simcDto2);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnFalse_Null()
    {
        //Arrange
        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = simcDto.Equals((object)null!);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnTrue_SameRef()
    {
        //Arrange
        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = simcDto.Equals((object)simcDto);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnFalse_DifferentType()
    {
        //Arrange
        var tercDto = new Faker<TercDto>()
            .Generate();

        var simcDto = new Faker<SimcDto>()
            .Generate();

        //Act
        var result = simcDto.Equals(tercDto);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_CompareToObject_ShouldReturnTrue_SameValue()
    {
        //Arrange
        var simcDto1 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 1)
            .Generate();

        var simcDto2 = new Faker<SimcDto>().RuleFor(x => x.Id, _ => 1)
            .Generate();

        //Act
        var result = simcDto1.Equals((object)simcDto2);

        //Assert
        Assert.That(result, Is.True);
    }
}