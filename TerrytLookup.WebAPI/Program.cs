using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TerrytLookup.Infrastructure.ExceptionHandling;
using TerrytLookup.Infrastructure.Extensions;
using TerrytLookup.Infrastructure.Models.Profiles;
using TerrytLookup.Infrastructure.Repositories.DbContext;
using TerrytLookup.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TerrytLookup API",
        Description = """
                      <h3> This is a web API serving Terryt data </h3>
                      <br />
                      Terryt data can be fed via CSV files, that can be downloaded from the link below
                      <br />
                      <a href="https://eteryt.stat.gov.pl/eTeryt/rejestr_teryt/udostepnianie_danych/baza_teryt/uzytkownicy_indywidualni/pobieranie/pliki_pelne.aspx">https://eteryt.stat.gov.pl/eTeryt</a>
                      """
    });
    
    options.OrderActionsBy(action => action.HttpMethod);

    options.IncludeXmlComments(Assembly.GetExecutingAssembly());
});

await builder.ConfigureDatabaseProvider();

builder.Services.RegisterProfiles();
builder.Services.RegisterApiServices();

builder.Services.AddProblemDetails(options => {
    options.CustomizeProblemDetails = context => {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

if (DatabaseProviderConfiguration.ConnectionString is null)
{
    throw new NullReferenceException("Database provider not initialized.");
}

builder.Services.AddHealthChecks()
    .AddNpgSql(DatabaseProviderConfiguration.ConnectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();

await context.Database.MigrateAsync();

app.MapHealthChecks("/_health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();