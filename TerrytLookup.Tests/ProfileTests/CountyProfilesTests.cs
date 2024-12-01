using AutoMapper;
using Bogus;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Profiles;

namespace TerrytLookup.Tests.ProfileTests;

public class CountyProfilesTests
{
    private static readonly MapperConfiguration
        Config = new(x => {
            x.AddProfile<BaseEntityProfiles>();
            x.AddProfile<CountyProfiles>();
            x.AddProfile<StreetProfiles>();
            x.AddProfile<TownProfiles>();
            x.AddProfile<VoivodeshipProfiles>();
        });

    private static readonly IMapper Mapper = Config.CreateMapper();

    [Test]
    public void CountyProfiles_ConfigurationShouldBeValid()
    {
        // Act
        Config.AssertConfigurationIsValid();
    }

    [Test]
    public void CountyProfiles_ShouldMapTercDtoToCreateDto()
    {
        //Arrange
        var entity = new Faker<TercDto>()
            .RuleFor(x => x.VoivodeshipId, faker => faker.Random.Int())
            .RuleFor(x => x.CountyId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<CreateCountyDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.TerrytId, Is.EqualTo((entity.VoivodeshipId, entity.CountyId)));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
        });
    }

    [Test]
    public void CountyProfiles_ShouldMapToDictionary()
    {
        //Arrange
        var entities = new Faker<TercDto>()
            .RuleFor(x => x.VoivodeshipId, faker => faker.Random.Int())
            .RuleFor(x => x.CountyId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate(10);

        //Act
        var mappedDict = Mapper.Map<Dictionary<(int, int), CreateCountyDto>>(entities);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedDict, Is.Not.Null);
            Assert.That(mappedDict, Has.Count.EqualTo(entities.Count));
        });
    }

    [Test]
    public void CountyProfiles_ShouldMapCreateDtoToDomain()
    {
        //Arrange
        var entity = new Faker<CreateCountyDto>()
            .RuleFor(x => x.TerrytId, faker => (faker.Random.Int(), faker.Random.Int()))
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<County>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.CountyId, Is.EqualTo(entity.TerrytId.countyId));
            Assert.That(mappedEntity.VoivodeshipId, Is.EqualTo(entity.TerrytId.voivodeshipId));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
        });
    }

    [Test]
    public void CountyProfiles_ShouldMapDomainToDto()
    {
        //Arrange
        var entity = new Faker<County>()
            .RuleFor(x => x.VoivodeshipId, faker => faker.Random.Int())
            .RuleFor(x => x.CountyId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<CountyDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.CountyId, Is.EqualTo(entity.CountyId));
            Assert.That(mappedEntity.VoivodeshipId, Is.EqualTo(entity.VoivodeshipId));
        });
    }
}