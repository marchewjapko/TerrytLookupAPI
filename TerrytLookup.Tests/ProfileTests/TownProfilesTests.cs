using AutoMapper;
using Bogus;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Profiles;

namespace TerrytLookup.Tests.ProfileTests;

public class TownProfilesTests
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
    public void TownProfiles_ShouldMapSimcDtoToCreateDto()
    {
        //Arrange
        var entity = new Faker<SimcDto>()
            .RuleFor(x => x.Id, faker => faker.IndexFaker)
            .RuleFor(x => x.VoivodeshipId, faker => faker.IndexFaker)
            .RuleFor(x => x.CountyId, faker => faker.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<CreateTownDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.TerrytId, Is.EqualTo(entity.Id));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
            Assert.That(mappedEntity.CountyTerrytId, Is.EqualTo((entity.VoivodeshipId, entity.CountyId)));
        });
    }

    [Test]
    public void TownProfiles_ShouldMapToDictionary()
    {
        //Arrange
        var entities = new Faker<SimcDto>()
            .RuleFor(x => x.Id, faker => faker.IndexFaker)
            .RuleFor(x => x.VoivodeshipId, faker => faker.IndexFaker)
            .RuleFor(x => x.CountyId, faker => faker.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate(10);

        //Act
        var mappedDict = Mapper.Map<Dictionary<int, CreateTownDto>>(entities);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedDict, Is.Not.Null);
            Assert.That(mappedDict, Has.Count.EqualTo(entities.Count));
        });
    }

    [Test]
    public void TownProfiles_ShouldMapCreateDtoToDomain()
    {
        //Arrange
        var entity = new Faker<CreateTownDto>()
            .RuleFor(x => x.TerrytId, faker => faker.IndexFaker)
            .RuleFor(x => x.CountyTerrytId, faker => (faker.IndexFaker, faker.IndexFaker))
            .RuleFor(x => x.ParentTownTerrytId, faker => faker.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.State())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<Town>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.Id, Is.EqualTo(entity.TerrytId));
            Assert.That(mappedEntity.ValidFromDate, Is.EqualTo(entity.ValidFromDate));
            Assert.That(mappedEntity.CountyId, Is.EqualTo(entity.CountyTerrytId.countyId));
            Assert.That(mappedEntity.CountyVoivodeshipId, Is.EqualTo(entity.CountyTerrytId.voivodeshipId));
        });
    }

    [Test]
    public void TownProfiles_ShouldMapDomainToDto()
    {
        //Arrange
        var entity = new Faker<Town>()
            .RuleFor(x => x.Id, faker => faker.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.State())
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<TownDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.Id, Is.EqualTo(entity.Id));
        });
    }
}