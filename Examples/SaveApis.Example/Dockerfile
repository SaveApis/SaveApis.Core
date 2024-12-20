FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Examples/SaveApis.Example/SaveApis.Example.csproj", "Examples/SaveApis.Example/"]
COPY ["SaveApis.Core/SaveApis.Core.csproj", "SaveApis.Core/"]
RUN dotnet restore "Examples/SaveApis.Example/SaveApis.Example.csproj"
COPY . .
WORKDIR "/src/Examples/SaveApis.Example"
RUN dotnet build "SaveApis.Example.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SaveApis.Example.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaveApis.Example.dll"]
