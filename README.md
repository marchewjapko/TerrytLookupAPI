# TerrytLookup API

TerrytLookupAPI is a RESTful web api made to query [Terryt Registry](https://eteryt.stat.gov.pl/eTeryt/english.aspx?contrast=default). The registry contains information regarding territorial divisions of Poland. <br/>
The purpose of the API is to allow other systems to use address data that is compliant with Terryt. This way they can avoid pitfalls of string-based and user inputted address information.

# Status
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marchewjapko_TerrytLookupAPI&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=marchewjapko_TerrytLookupAPI)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=marchewjapko_TerrytLookupAPI&metric=coverage)](https://sonarcloud.io/summary/new_code?id=marchewjapko_TerrytLookupAPI)

# Components

- WebAPI
- Database

# Technologies

- [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [PostgreSQL](https://www.postgresql.org/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
    - [Npgsql Entity Framework Core Provider](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
    - [EFCore.BulkExtensions](https://www.nuget.org/packages/EFCore.BulkExtensions)
- [CsvHelper](https://www.nuget.org/packages/CsvHelper)
- [NUnit](https://www.nuget.org/packages/NUnit)
- [Testcontainers](https://www.nuget.org/packages/Testcontainers) - an excellent solution for unit tests! <sub>bye bye in-memory</sub>

The entire system is contained in docker containers for easy deployment. 

# Docker

To build the image:

    docker build -t [docker username]/terryt-lookup-api-web-api -f .\TerrytLookup.WebAPI\Dockerfile .

To publish the image:

    docker push marchewjapko/terryt-lookup-api_web-api:latest

To run compose:

    docker compose --env-file .env up -d

# Important considerations

Terryt registry has an odd data scheme. E.g.: counties do not have any single unique identifier, thusly the system relies on composite keys for some entities 
