# TerrytLookup API

TerrytLookupAPI is a RESTful web api made to query [Terryt Registry](https://eteryt.stat.gov.pl/eTeryt/english.aspx?contrast=default). The registry contains information regarding territorial divisions of Poland. <br/>
The purpose of the API is to allow other systems to use address data that is compliant with Terryt. This way they can avoid pitfalls of string-based and user inputted address information.

# Status
[ToDo]

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

# Important considerations

Terryt registry has an odd data scheme. E.g.: counties do not have any single unique identifier, thusly the system relies on composite keys for some entities