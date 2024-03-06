# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore project files
COPY *.csproj ./
RUN dotnet restore

# Copy and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# SQL Server Stage
FROM mcr.microsoft.com/mssql/server:2022-latest AS sqlserver
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=$SA_PASSWORD

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the application from the build stage
COPY --from=build /app/out ./

# Copy the SQL Server data directory
COPY --from=sqlserver /var/opt/mssql/data /var/opt/mssql/data

# Expose SQL Server port
EXPOSE 1433

# Set the entrypoint for SQL Server
ENTRYPOINT /opt/mssql/bin/sqlservr

# Expose the application port
EXPOSE 80

# Start the application
CMD ["dotnet", "ChatHub.dll"]