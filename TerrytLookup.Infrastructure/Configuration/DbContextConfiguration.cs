using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Infrastructure.Repositories.DbContext;
using Testcontainers.PostgreSql;

namespace TerrytLookup.Infrastructure.Configuration;

public static class DbContextConfiguration
{
    public static async Task ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["DatabaseType"] == "SingleUse")
        {
            await ConfigureSingleUseDb(services, configuration);

            return;
        }

        ConfigurePersistentDb(services, configuration);
    }

    private static void ConfigurePersistentDb(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(AppDbContext.ConnectionStringSectionName);

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    }

    private static async Task ConfigureSingleUseDb(IServiceCollection services, IConfiguration configuration)
    {
        var password = Guid
            .NewGuid()
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

        var connectionString = dbContainer.GetConnectionString();
        connectionString += "; Include Error Detail=True";

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        configuration[$"ConnectionStrings:{AppDbContext.ConnectionStringSectionName}"] = connectionString;

#if DEBUG
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine("New database configuration:");
        Console.WriteLine($"Connection string: {dbContainer.GetConnectionString()}");
        Console.WriteLine("--------------------------------------------------------");
#endif
    }
}