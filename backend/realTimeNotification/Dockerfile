FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8090

ENV ASPNETCORE_URLS=https://+:8090
ENV ASPNETCORE_HTTPS_PORT=8090

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["backend/realTimeNotification/realTimeNotification.csproj", "./"]
RUN dotnet restore "realTimeNotification.csproj"
COPY backend/realTimeNotification .
WORKDIR "/src/."
RUN dotnet build "realTimeNotification.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "realTimeNotification.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Generate HTTPS development certificate
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p password

# Expose the HTTPS port
EXPOSE 8090

# Set the environment variables for HTTPS
ENV ASPNETCORE_URLS=https://+:8090
ENV ASPNETCORE_HTTPS_PORT=8090
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "realTimeNotification.dll"]