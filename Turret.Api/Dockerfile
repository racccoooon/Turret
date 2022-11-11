FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Turret.Api/Turret.Api.csproj", "Turret.Api/"]
RUN dotnet restore "Turret.Api/Turret.Api.csproj"
COPY . .
WORKDIR "/src/Turret.Api"
RUN dotnet build "Turret.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Turret.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Turret.Api.dll"]
