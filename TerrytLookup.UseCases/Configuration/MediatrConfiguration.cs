using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TerrytLookup.UseCases.Configuration;

public static class MediatrConfiguration
{
    public static void RegisterMediatr(this IServiceCollection services)
    {
        services.AddMediatR(
            configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}