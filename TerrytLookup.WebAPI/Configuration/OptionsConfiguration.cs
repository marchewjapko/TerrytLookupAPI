using TerrytLookup.Core.Options;

namespace TerrytLookup.WebAPI.Configuration;

public static class OptionsConfiguration
{
    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterCodeOptions(configuration);
    }

    private static void RegisterCodeOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RepositoryOptions>(configuration.GetSection(nameof(RepositoryOptions)));
    }
}