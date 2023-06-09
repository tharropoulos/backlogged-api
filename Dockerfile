FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG PGHOST
ARG PGPASSWORD
ARG PGPORT
ARG PGUSER
ARG PGDATABASE
ARG JWT_AUDIENCE
ARG JWT_ISSUER
ARG JWT_SECRET

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["backlogged-api.csproj", "./"]
RUN dotnet restore "backlogged-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "backlogged-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backlogged-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV "JwtSettings__Audience"="${JWT_AUDIENCE}"
ENV "JwtSettings__Issuer"="${JWT_ISSUER}"
ENV "JwtSettings__Key"="${JWT_SECRET}"
ENV "ConnectionStrings__DefaultConnection"="Host=${PGHOST};Port=${PGPORT};Username=${PGUSER};Password=${PGPASSWORD};Database=${PGDATABASE}"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backlogged-api.dll"]