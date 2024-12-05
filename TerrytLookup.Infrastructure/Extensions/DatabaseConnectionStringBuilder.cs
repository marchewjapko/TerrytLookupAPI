using Microsoft.Extensions.Configuration;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;

namespace TerrytLookup.Infrastructure.Extensions;

public static class DatabaseConnectionStringBuilder
{
    private const string ConfigurationPath = "DbConnection:DefaultConnection";

    private const string ConfigurationNameHost = "Host";
    private const string ConfigurationNamePort = "Port";
    private const string ConfigurationNameUser = "User";
    private const string ConfigurationNamePassword = "Password";

    private const string EnvironmentVariableUser = "POSTGRES_USER";
    private const string EnvironmentVariablePassword = "POSTGRES_PASSWORD";

    public static string GetConnectionString(this IConfiguration configuration)
    {
        if (configuration["DatabaseType"] == "SingleUse")
        {
            throw new InvalidDatabaseConfigurationException("SingleUse database doesn't have an explicit connection string.");
        }

        var user = TryGetFromConfigurationOrEnvironment(configuration, ConfigurationNameUser, EnvironmentVariableUser);
        if (user is null)
        {
            throw new InvalidDatabaseConfigurationException(
                $"'{ConfigurationPath}:{ConfigurationNameUser}' value missing in configuration and no \'POSTGRES_USER\' in environment variables");
        }

        var password = TryGetFromConfigurationOrEnvironment(configuration, ConfigurationNamePassword, EnvironmentVariablePassword);
        if (password is null)
        {
            throw new InvalidDatabaseConfigurationException(
                $"'{ConfigurationPath}:{ConfigurationNamePassword}' value missing in configuration and no \'POSTGRES_PASSWORD\' in environment variables");
        }

        var host = configuration[$"{ConfigurationPath}:{ConfigurationNameHost}"];
        if (host is null)
        {
            throw new InvalidDatabaseConfigurationException($"Value missing from configuration: {ConfigurationPath}:{ConfigurationNameHost}.");
        }

        var port = configuration[$"{ConfigurationPath}:{ConfigurationNamePort}"];

        var connectionString = $"Host={host};Username={user};Password={password}";

        if (port is not null)
        {
            connectionString += $";Port={port}";
        }

        return connectionString;
    }

    private static string? TryGetFromConfigurationOrEnvironment(
        IConfiguration configuration,
        string settingName,
        string environmentVariable)
    {
        var configurationValue = configuration[$"{ConfigurationPath}:{settingName}"];
        if (!string.IsNullOrWhiteSpace(configurationValue))
        {
            return configurationValue;
        }

        var variableValue = Environment.GetEnvironmentVariable(environmentVariable);
        if (!string.IsNullOrWhiteSpace(variableValue))
        {
            return variableValue;
        }

        return null;
    }
}