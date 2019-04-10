FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["defibrillator-service/defibrillator-service.csproj", "defibrillator-service/"]
RUN dotnet restore "defibrillator-service/defibrillator-service.csproj"
COPY . .
WORKDIR "/src/defibrillator-service"
RUN dotnet build "defibrillator-service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "defibrillator-service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "defibrillator-service.dll"]