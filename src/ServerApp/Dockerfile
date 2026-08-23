# Multi-stage build for ServerApp (.NET Minimal API), used by Render (Docker runtime).
# Render's "root directory" setting for this service is src/ServerApp — this file is built
# with that folder as the build context, not the repo root.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ServerApp.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render injects PORT at container start; 10000 is only the local/default fallback.
ENV PORT=10000
EXPOSE 10000

# Binds Kestrel to 0.0.0.0 (not localhost) on whatever port PORT resolves to, so Render's
# reverse proxy can reach the container.
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:$PORT dotnet ServerApp.dll"]
