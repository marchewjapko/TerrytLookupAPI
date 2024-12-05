using Microsoft.Extensions.Configuration;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;
using TerrytLookup.Infrastructure.Extensions;

namespace TerrytLookup.Tests.ExtensionTests;

public class DatabaseConnectionStringBuilderTests
{
    [Test]
    public void GetConnectionString_ShouldThrowInvalidDatabaseConfigurationExceptionWhenDatabaseIsSingleUse()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>()
                {
                    new("DatabaseType", "SingleUse")
                })
            .Build();

        //Act
        var exception = Assert.Throws<InvalidDatabaseConfigurationException>(
            () => configuration.GetConnectionString());

        //Assert
        Assert.That(exception.Message, Is.EqualTo("SingleUse database doesn't have an explicit connection string."));
    }
    
    [Test]
    public void GetConnectionString_InvalidDatabaseConfigurationExceptionWhenNoUser()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var exception = Assert.Throws<InvalidDatabaseConfigurationException>(
            () => configuration.GetConnectionString());

        //Assert
        Assert.That(exception.Message, Is.EqualTo("'DbConnection:DefaultConnection:User' value missing in configuration and no 'POSTGRES_USER' in environment variables"));
    }
    
    [Test]
    public void GetConnectionString_InvalidDatabaseConfigurationExceptionWhenNoPassword()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>()
                {
                    new("DbConnection:DefaultConnection:User", "User")
                })
            .Build();

        //Act
        var exception = Assert.Throws<InvalidDatabaseConfigurationException>(
            () => configuration.GetConnectionString());

        //Assert
        Assert.That(exception.Message, Is.EqualTo("'DbConnection:DefaultConnection:Password' value missing in configuration and no 'POSTGRES_PASSWORD' in environment variables"));
    }
    
    [Test]
    public void GetConnectionString_InvalidDatabaseConfigurationExceptionWhenNoHost()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>()
                {
                    new("DbConnection:DefaultConnection:User", "User"),
                    new("DbConnection:DefaultConnection:Password", "123")
                })
            .Build();

        //Act
        var exception = Assert.Throws<InvalidDatabaseConfigurationException>(
            () => configuration.GetConnectionString());

        //Assert
        Assert.That(exception.Message, Is.EqualTo($"Value missing from configuration: DbConnection:DefaultConnection:Host."));
    }
    
    [Test]
    public void GetConnectionString_ShouldReturnConnectionString()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>()
                {
                    new("DbConnection:DefaultConnection:User", "User"),
                    new("DbConnection:DefaultConnection:Host", "localhost")
                })
            .Build();
        
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "123");

        //Act
        var result = configuration.GetConnectionString();

        //Assert
        Assert.That(result, Is.EqualTo("Host=localhost;Username=User;Password=123"));
    }
    
    [Test]
    public void GetConnectionString_ShouldReturnConnectionString_WithPort()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>()
                {
                    new("DbConnection:DefaultConnection:User", "User"),
                    new("DbConnection:DefaultConnection:Host", "localhost"),
                    new("DbConnection:DefaultConnection:Port", "1")
                })
            .Build();
        
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "123");

        //Act
        var result = configuration.GetConnectionString();

        //Assert
        Assert.That(result, Is.EqualTo("Host=localhost;Username=User;Password=123;Port=1"));
    }
}