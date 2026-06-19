# Backend .NET 10 — Multi-stage build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/ src/

RUN dotnet restore src/Host/PlateformeCVTech.Api.csproj
RUN dotnet publish src/Host/PlateformeCVTech.Api.csproj -c Release -o /app/publish --no-restore

# Image runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "PlateformeCVTech.Api.dll"]
