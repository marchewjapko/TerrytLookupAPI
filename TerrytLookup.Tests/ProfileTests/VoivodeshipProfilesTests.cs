using AutoMapper;
using Bogus;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Profiles;

namespace TerrytLookup.Tests.ProfileTests;

public class VoivodeshipProfilesTests
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
    public void VoivodeshipProfiles_ConfigurationShouldBeValid()
    {
        // Act
        Config.AssertConfigurationIsValid();
    }

    [Test]
    public void VoivodeshipProfiles_ShouldMapTercDtoToCreateDto()
    {
        //Arrange
        var entity = new Faker<TercDto>()
            .RuleFor(x => x.VoivodeshipId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<CreateVoivodeshipDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.TerrytId, Is.EqualTo(entity.VoivodeshipId));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
        });
    }

    [Test]
    public void VoivodeshipProfiles_ShouldMapToDictionary()
    {
        //Arrange
        var entities = new Faker<TercDto>()
            .RuleFor(x => x.VoivodeshipId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate(10);

        //Act
        var mappedDict = Mapper.Map<Dictionary<int, CreateVoivodeshipDto>>(entities);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedDict, Is.Not.Null);
            Assert.That(mappedDict, Has.Count.EqualTo(entities.Count));
        });
    }

    [Test]
    public void VoivodeshipProfiles_ShouldMapCreateDtoToDomain()
    {
        //Arrange
        var entity = new Faker<CreateVoivodeshipDto>()
            .RuleFor(x => x.TerrytId, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<Voivodeship>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.Id, Is.EqualTo(entity.TerrytId));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
        });
    }

    [Test]
    public void VoivodeshipProfiles_ShouldMapDomainToDto()
    {
        //Arrange
        var entity = new Faker<Voivodeship>()
            .RuleFor(x => x.Id, faker => faker.Random.Int())
            .RuleFor(x => x.Name, f => f.Address.State())
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<VoivodeshipDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.Id, Is.EqualTo(entity.Id));
        });
    }
}