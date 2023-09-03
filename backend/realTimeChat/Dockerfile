FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_URLS=http://+:8000

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["backend/realTimeChat/realTimeServices.csproj", "./"]
RUN dotnet restore "realTimeServices.csproj"
COPY backend/realTimeChat .
WORKDIR "/src/."
RUN dotnet build "realTimeServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "realTimeServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "realTimeServices.dll"]


