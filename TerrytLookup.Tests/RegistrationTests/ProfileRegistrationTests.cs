﻿using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Infrastructure.Models.Profiles;

namespace TerrytLookup.Tests.RegistrationTests;

public class ProfileRegistrationTests
{
    [Test]
    public void ShouldRegisterProfileServices()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        var assembly = Assembly.Load("TerrytLookup.Infrastructure");
        var types = assembly.GetTypes();

        var profileTypes = types.Where(type => type is
            { Namespace: "TerrytLookup.Infrastructure.Models.Profiles", IsPublic: true, IsAbstract: false });

        // Act
        services.RegisterProfiles();
        var serviceProvider = services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        var configuration = mapper.ConfigurationProvider;
        var registeredProfiles = configuration.Internal()
            .Profiles;

        // Assert
        Assert.That(profileTypes.Select(x => x.ToString())
            .All(x => registeredProfiles.Select(a => a.Name)
                .Contains(x)), Is.True);
    }
}