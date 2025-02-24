using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.Repositories;

namespace TerrytLookup.Infrastructure.Configuration;

public static class RepositoryConfiguration
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVoivodeshipRepository, VoivodeshipRepository>();
        services.AddScoped<ICountyRepository, CountyRepository>();
        services.AddScoped<ITownRepository, TownRepository>();
        services.AddScoped<IStreetRepository, StreetRepository>();
    }
}