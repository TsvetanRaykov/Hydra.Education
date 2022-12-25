FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Container

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "Hydra.Education.sln"
WORKDIR "/src/Hydra.Server.Auth"
RUN dotnet build "Hydra.Server.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hydra.Server.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p storage/sql
RUN mkdir -p storage/files

ENTRYPOINT ["dotnet", "Hydra.Server.Auth.dll"]
