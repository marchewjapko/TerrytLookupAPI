using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.WebAPI.Configuration;

public static class HealthChecksConfiguration
{
    public static void RegisterHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(AppDbContext.ConnectionStringSectionName);

        if (connectionString is null)
            throw new NullReferenceException("The connection string is null.");

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString, timeout: TimeSpan.FromSeconds(10));

        services.AddGrpcHealthChecks(options => options.Services.Map("npgsql", context => context.Name == "npgsql"));
    }

    public static void UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks(
            "/_health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        app.MapGrpcHealthChecksService();
    }
}