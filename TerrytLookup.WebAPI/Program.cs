using FastEndpoints;
using TerrytLookup.Infrastructure.Configuration;
using TerrytLookup.UseCases.Configuration;
using TerrytLookup.WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.ConfigureDbContext(builder.Configuration);

builder.ConfigureGrpc();
builder.ConfigureSwagger();
builder.Services.AddFastEndpoints();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.RegisterMediatr();
builder.Services.RegisterOptions(builder.Configuration);
builder.Services.RegisterHealthChecks(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseGrpcServices();
app.UseSwagger(builder);

app.UseHealthChecks();
app.UseFastEndpoints();

await app.MigrateToLatestMigration();

await app.RunAsync();