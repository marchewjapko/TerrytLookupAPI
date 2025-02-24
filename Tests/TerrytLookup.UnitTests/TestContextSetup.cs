using Microsoft.EntityFrameworkCore;
using TerrytLookup.Infrastructure.Repositories.DbContext;
using Testcontainers.PostgreSql;

namespace TerrytLookup.UnitTests;

internal static class TestContextSetup
{
    public static async Task<AppDbContext> SetupAsync()
    {
        var password = Guid
            .NewGuid()
            .ToString();

        var name = $"TerrytLookup-API-test-database-{Guid.NewGuid()
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

        var contextOptions = new DbContextOptionsBuilder<AppDbContext>();
        contextOptions.UseNpgsql(connectionString);

        var newContext = new AppDbContext(contextOptions.Options);

        await newContext.Database.MigrateAsync();

        return newContext;
    }
}