using System.Reflection;
using Microsoft.OpenApi.Models;

namespace TerrytLookup.WebAPI.Configuration;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcSwagger();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = $"{builder.Environment.ApplicationName} v1",
                        Description = """
                                      <h3> This is a web API serving Terryt data </h3>
                                      <br />
                                      Terryt data can be fed via CSV files, that can be downloaded from the link below
                                      <br />
                                      <a href="https://eteryt.stat.gov.pl/eTeryt/rejestr_teryt/udostepnianie_danych/baza_teryt/uzytkownicy_indywidualni/pobieranie/pliki_pelne.aspx">https://eteryt.stat.gov.pl/eTeryt</a>
                                      """,
                        Version = "v1"
                    });

                options.OrderActionsBy(action => action.HttpMethod);

                var filePath = Path.Combine(AppContext.BaseDirectory, "TerrytLookup.WebAPI.xml");

                options.IncludeXmlComments(filePath);

                options.IncludeGrpcXmlComments(filePath);
            });
    }

    public static void UseSwagger(this WebApplication app, WebApplicationBuilder builder)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            c =>
            {
                c.EnableTryItOutByDefault();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
            });
    }
}