using System.Collections.Concurrent;
using Bogus;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;

namespace TerrytLookup.Tests.DtoTests;

public class CreateTownDtoTests
{
    [Test]
    public void CopyStreetsTo_ShouldCopyStreets()
    {
        //Arrange
        var town1 = new Faker<CreateTownDto>()
            .RuleFor(x => x.Streets, () => new ConcurrentBag<CreateStreetDto>(new Faker<CreateStreetDto>().Generate(2)))
            .Generate();

        var town2 = new Faker<CreateTownDto>().Generate();

        Assume.That(town2.Streets, Is.Empty);

        //Act
        town1.CopyStreetsTo(town2);

        //Assert
        Assert.That(town1.Streets, Is.EquivalentTo(town2.Streets));
    }
}