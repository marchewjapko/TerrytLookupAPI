using Microsoft.EntityFrameworkCore;
using TerrytLookup.Infrastructure.Repositories.DbContext;
using Testcontainers.PostgreSql;

namespace TerrytLookup.WebAPI;

/// <summary>
///     Contains extension method <see cref="ConfigureDatabaseProvider" /> for database provider configuration.
/// </summary>
public static class DatabaseProviderConfiguration
{
    /// <summary>
    ///     Configures the database provider for the application based on the specified database type in the configuration.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder" /> instance used to configure the application services.</param>
    /// <remarks>
    ///     This method checks the "DatabaseType" configuration setting and configures the database context accordingly:
    ///     <list type="bullet">
    ///         <item>
    ///             If "DatabaseType" is "SingleUse", it creates a PostgreSQL container with a generated password and starts
    ///             it,
    ///             then configures the database context to use the connection string from the container. <br />
    ///             Newly created container should be automatically deleted when the application exits.
    ///         </item>
    ///         <item>
    ///             Otherwise, it configures the database context to use the connection string specified in
    ///             the "DbConnectionString" configuration setting.
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task ConfigureDatabaseProvider(this WebApplicationBuilder builder)
    {
        if (builder.Configuration["DatabaseType"] == "SingleUse")
        {
            await ConfigureSingleUseDb(builder);

            return;
        }

        ConfigurePersistentDb(builder);
    }

    private static void ConfigurePersistentDb(WebApplicationBuilder builder)
    {
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");

        var connectionString = string.Format(builder.Configuration.GetConnectionString("DbConnectionString")!, username, password);

        var envs = Environment.GetEnvironmentVariables();

        Console.WriteLine("--envs--");
        foreach (var env in envs)
        {
            Console.WriteLine($"env: {env}");
        }
        
        Console.WriteLine($"Using PostgreSQL connection: {connectionString}");
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static async Task ConfigureSingleUseDb(WebApplicationBuilder builder)
    {
        var password = Guid.NewGuid()
            .ToString();

        var name = $"TerrytLookup-API-runtime-database-{Guid.NewGuid()
            .ToString()}";

        var dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithPassword(password)
            .WithName(name)
            .WithAutoRemove(true)
            .Build();

        await dbContainer.StartAsync();

        var containerConnectionString = dbContainer.GetConnectionString();
        containerConnectionString += "; Include Error Detail=True";

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(containerConnectionString));

#if DEBUG
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine("New database configuration:");
        Console.WriteLine($"Connection string: {dbContainer.GetConnectionString()}");
        Console.WriteLine("--------------------------------------------------------");
#endif
    }
}