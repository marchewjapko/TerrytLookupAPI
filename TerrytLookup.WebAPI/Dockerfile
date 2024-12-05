FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TerrytLookup.WebAPI/TerrytLookup.WebAPI.csproj", "TerrytLookup.WebAPI/"]
RUN dotnet restore "TerrytLookup.WebAPI/TerrytLookup.WebAPI.csproj"

COPY "./TerrytLookup.Core" "./TerrytLookup.Core"
COPY "./TerrytLookup.Infrastructure" "./TerrytLookup.Infrastructure"
COPY "./TerrytLookup.WebAPI" "./TerrytLookup.WebAPI"

WORKDIR "/src/TerrytLookup.WebAPI"
RUN dotnet build "TerrytLookup.WebAPI.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "TerrytLookup.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get -y update; apt-get -y install curl
RUN adduser --disabled-password myuser
USER myuser
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TerrytLookup.WebAPI.dll"]

HEALTHCHECK --interval=1m --timeout=10s --retries=3 CMD curl -f $HEALTH_CHECK_PATH || exit 1