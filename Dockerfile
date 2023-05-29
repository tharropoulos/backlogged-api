
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["backlogged-api.csproj", "backlogged-api/"]
RUN dotnet restore "backlogged-api/backlogged-api.csproj"
COPY . .
WORKDIR "/src/backlogged-api"
RUN dotnet build "backlogged-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backlogged-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backlogged-api.dll"]