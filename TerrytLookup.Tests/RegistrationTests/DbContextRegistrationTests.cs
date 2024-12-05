using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Infrastructure.Extensions;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Tests.RegistrationTests;

public class DatabaseProviderConfigurationTests
{
    [Test]
    public async Task ConfigureDatabaseProvider_ShouldConfigureSingleUse()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        
        builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string?>>()
        {
            new("DatabaseType", "SingleUse")
        });
        
        // Act
        await builder.ConfigureDatabaseProvider();
        
        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();
        
        // Assert
        Assert.That(builder.Services.ToList().Any(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>)), Is.True);
    }

    [Test]
    public async Task ConfigureDatabaseProvider_ShouldUseAppSettings()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        
        builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string?>>()
        {
            new("DatabaseType", "Persistent"),
            new("DbConnection:DefaultConnection:Host", "localhost"),
            new("DbConnection:DefaultConnection:Port", "1"),
            new("DbConnection:DefaultConnection:User", "user"),
            new("DbConnection:DefaultConnection:Password", "123"),
        });
        
        // Act
        await builder.ConfigureDatabaseProvider();
        
        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();
        
        // Assert
        Assert.That(builder.Services.ToList().Any(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>)), Is.True);
    }
}