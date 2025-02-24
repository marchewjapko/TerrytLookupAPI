ARG BUILD_CONFIGURATION="Production"

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Main endpoint for TSL gRPC channels
ENV Kestrel__Endpoints__Https__Url=https://+:8080
EXPOSE 8080

#Endpoint for unsecured gRPC channels
ENV Kestrel__Endpoints__Http__Url=http://+:8081
EXPOSE 8081

#Endpoint unsuitable for gRPC clients, suitable for healthcheck calls
ENV Kestrel__Endpoints__Http1AndHttp2__Url=http://+:8082
ENV Kestrel__Endpoints__Http1AndHttp2__Protocols=Http1AndHttp2
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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
WORKDIR /app

USER root
RUN chown $APP_UID:app .
USER $APP_UID

COPY --from=publish /app/publish .
RUN ["sh", "-c", "apt-get -y update && apt-get --no-install-recommends -y install curl && apt-get clean && useradd -m $APP_UID"]
USER $APP_UID
ENTRYPOINT ["dotnet", "TerrytLookup.WebAPI.dll"]

ENV HEALTH_CHECK_PATH="localhost:8082/_health"
HEALTHCHECK --interval=1m --timeout=10s --retries=3 CMD ["curl", "-f", "$HEALTH_CHECK_PATH", "|| exit 1"]