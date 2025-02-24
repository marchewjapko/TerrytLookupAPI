using Microsoft.EntityFrameworkCore;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.WebAPI.Configuration;

public static class MigrationConfiguration
{
    public static async Task MigrateToLatestMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();
    }
}