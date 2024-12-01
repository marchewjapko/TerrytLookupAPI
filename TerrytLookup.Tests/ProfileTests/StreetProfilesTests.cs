using AutoMapper;
using Bogus;
using TerrytLookup.Core.Domain;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Models.Dto.Internal.CreateDtos;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Models.Profiles;

namespace TerrytLookup.Tests.ProfileTests;

public class StreetProfilesTests
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
    public void StreetProfiles_ConfigurationShouldBeValid()
    {
        // Act
        Config.AssertConfigurationIsValid();
    }

    [Test]
    public void StreetProfiles_ShouldMapTercDtoToCreateDto()
    {
        //Arrange
        var entity = new Faker<UlicDto>()
            .RuleFor(x => x.TownId, faker => faker.Random.Int())
            .RuleFor(x => x.StreetNameId, faker => faker.Random.Int())
            .RuleFor(x => x.StreetPrefix, _ => "ul.")
            .RuleFor(x => x.StreetNameSecondPart, f => f.Address.StreetName())
            .RuleFor(x => x.StreetNameFirstPart, f => f.Address.StreetName())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<CreateStreetDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo($"{entity.StreetPrefix} {entity.StreetNameSecondPart} {entity.StreetNameFirstPart}"));
            Assert.That(mappedEntity.TerrytTownId, Is.EqualTo(entity.TownId));
            Assert.That(mappedEntity.TerrytNameId, Is.EqualTo(entity.StreetNameId));
        });
    }

    [Test]
    public void StreetProfiles_ShouldMapCreateDtoToDomain()
    {
        //Arrange
        var entity = new Faker<CreateStreetDto>()
            .RuleFor(x => x.TerrytTownId, faker => faker.IndexFaker)
            .RuleFor(x => x.TerrytNameId, faker => faker.IndexFaker)
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<Street>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
            Assert.That(mappedEntity.NameId, Is.EqualTo(entity.TerrytNameId));
            Assert.That(mappedEntity.TownId, Is.EqualTo(entity.TerrytTownId));
        });
    }

    [Test]
    public void StreetProfiles_ShouldMapDomainToDto()
    {
        //Arrange
        var entity = new Faker<Street>()
            .RuleFor(x => x.Name, f => f.Address.StreetName())
            .RuleFor(x => x.NameId, f => f.IndexFaker)
            .RuleFor(x => x.TownId, f => f.IndexFaker)
            .RuleFor(x => x.ValidFromDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .Generate();

        //Act
        var mappedEntity = Mapper.Map<StreetDto>(entity);

        //Assert
        Assert.Multiple(() => {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.TownId, Is.EqualTo(entity.TownId));
            Assert.That(mappedEntity.NameId, Is.EqualTo(entity.NameId));
            Assert.That(mappedEntity.Name, Is.EqualTo(entity.Name));
        });
    }
}