FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["prueba-tecnica/prueba-tecnica.csproj", "prueba-tecnica/"]
RUN dotnet restore "prueba-tecnica/prueba-tecnica.csproj"
COPY . .
WORKDIR "/src/prueba-tecnica"
RUN dotnet build "prueba-tecnica.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "prueba-tecnica.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "prueba-tecnica.dll"]
