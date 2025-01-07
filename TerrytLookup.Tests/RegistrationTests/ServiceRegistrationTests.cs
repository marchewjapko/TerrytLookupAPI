using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Infrastructure.Services;

namespace TerrytLookup.Tests.RegistrationTests;

public class ServiceRegistrationTests
{
    [Test]
    public void ShouldRegisterApiServices()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        var servicesLocations = new List<(string, string)>
        {
            ("TerrytLookup.Core", "TerrytLookup.Core.Repositories"),
            ("TerrytLookup.Infrastructure", "TerrytLookup.Infrastructure.Services")
        };

        var numberOfExpectedInterfaces = (from location in servicesLocations
            let assembly = Assembly.Load(location.Item1)
            let types = assembly.GetTypes()
            let memberNamespaces =
                types.Where(t => t.Namespace != null && t.Namespace.StartsWith(location.Item2)).Select(t => t.Namespace)
                    .Distinct()
            select types.Count(type => type.IsInterface && memberNamespaces.Contains(type.Namespace))).Sum();

        // Act
        services.RegisterApiServices();

        // Assert
        Assert.That(services, Has.Count.EqualTo(numberOfExpectedInterfaces));
    }
}