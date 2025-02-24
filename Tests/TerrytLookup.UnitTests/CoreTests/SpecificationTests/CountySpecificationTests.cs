using Bogus;
using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Specifications;

namespace TerrytLookup.UnitTests.CoreTests.SpecificationTests;

public class CountySpecificationTests
{
    private readonly Faker _faker = new();

    [Test]
    public void CountyGetByFilterSpecification_ShouldFilter()
    {
        //Arrange
        const int pageSize = 10;
        const string filterName = "filterName";
        const int voivodeshipId = 1;
        var specification = new CountyGetByFilterSpecification(pageSize, filterName, voivodeshipId);

        var county = new County()
        {
            Name = filterName,
            VoivodeshipId = voivodeshipId,
            CountyId = _faker.Random.Int(1)
        };

        //Act
        var result = specification.IsSatisfiedBy(county);

        //Assert
        Assert.That(result, Is.True);
    }
}