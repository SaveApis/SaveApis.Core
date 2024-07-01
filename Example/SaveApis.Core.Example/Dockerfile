FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Example/SaveApis.Core.Example/SaveApis.Core.Example.csproj", "Example/SaveApis.Core.Example/"]
RUN dotnet restore "Example/SaveApis.Core.Example/SaveApis.Core.Example.csproj"
COPY . .
WORKDIR "/src/Example/SaveApis.Core.Example"
RUN dotnet build "SaveApis.Core.Example.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SaveApis.Core.Example.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaveApis.Core.Example.dll"]
