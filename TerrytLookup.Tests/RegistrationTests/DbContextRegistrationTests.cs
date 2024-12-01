using AutoMapper;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TerrytLookup.Infrastructure.Repositories.DbContext;
using TerrytLookup.WebAPI;

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
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var zz = dbContext.Database.GetConnectionString();
        
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
            new("ConnectionStrings:DbConnectionString", "PeePeePooPoo"),
        });
        
        // Act
        await builder.ConfigureDatabaseProvider();
        
        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var zz = dbContext.Database.GetConnectionString();
        
        // Assert
        Assert.That(builder.Services.ToList().Any(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>)), Is.True);
    }
}