using Microsoft.Extensions.DependencyInjection;
using TerrytLookup.Core.Interfaces;
using TerrytLookup.Core.Specifications;
using TerrytLookup.Infrastructure.Services;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.Infrastructure.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStreamReaderService<TercDto>, FileStreamReaderService<TercDto>>();
        services.AddScoped<IFileStreamReaderService<SimcDto>, FileStreamReaderService<SimcDto>>();
        services.AddScoped<IFileStreamReaderService<UlicDto>, FileStreamReaderService<UlicDto>>();
    }
}