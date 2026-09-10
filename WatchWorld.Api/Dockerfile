FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WatchWorld.Api/WatchWorld.Api.csproj WatchWorld.Api/
COPY WatchWorld.Application/WatchWorld.Application.csproj WatchWorld.Application/
COPY WatchWorld.Domain/WatchWorld.Domain.csproj WatchWorld.Domain/
COPY WatchWorld.Infrastructure/WatchWorld.Infrastructure.csproj WatchWorld.Infrastructure/
RUN dotnet restore WatchWorld.Api/WatchWorld.Api.csproj

COPY . .
RUN dotnet publish WatchWorld.Api/WatchWorld.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WatchWorld.Api.dll"]
