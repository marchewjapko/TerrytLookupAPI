using Google.Protobuf.Reflection;
using Google.Rpc;
using Microsoft.AspNetCore.Grpc.JsonTranscoding;
using TerrytLookup.WebAPI.Middlewares;
using TerrytLookup.WebAPI.Services;

namespace TerrytLookup.WebAPI.Configuration;

public static class GrpcConfiguration
{
    public static void ConfigureGrpc(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddGrpc(
                options =>
                {
                    options.EnableDetailedErrors = true;
                    options.Interceptors.Add<ExceptionHandlingInterceptor>();
                })
            .AddJsonTranscoding(
                options =>
                {
                    options.JsonSettings = new GrpcJsonSettings
                    {
                        WriteIndented = true
                    };

                    options.TypeRegistry = TypeRegistry.FromMessages(ErrorInfo.Descriptor);
                });

        if (builder.Environment.IsDevelopment())
            builder.Services.AddGrpcReflection();

        builder.Services.AddGrpcSwagger();
    }

    public static void UseGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<VoivodeshipService>();
        app.MapGrpcService<CountyService>();
        app.MapGrpcService<TownService>();
        app.MapGrpcService<StreetService>();

        app.MapGrpcReflectionService();
    }
}