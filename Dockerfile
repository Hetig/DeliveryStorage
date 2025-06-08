FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DeliveryStorage.API/DeliveryStorage.API.csproj", "DeliveryStorage.API/"]
COPY ["DeliveryStorage.Database/DeliveryStorage.Database.csproj", "DeliveryStorage.Database/"]
COPY ["DeliveryStorage.Domain/DeliveryStorage.Domain.csproj", "DeliveryStorage.Domain/"]
RUN dotnet restore "DeliveryStorage.API/DeliveryStorage.API.csproj"
COPY . .
WORKDIR "/src/DeliveryStorage.API"
RUN dotnet build "./DeliveryStorage.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeliveryStorage.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryStorage.API.dll"]
