FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["examples/Example.Web/Example.Web.csproj", "examples/Example.Web/"]
COPY ["src/SaveApis.Core.Web/SaveApis.Core.Web.csproj", "src/SaveApis.Core.Web/"]
COPY ["src/SaveApis.Core.Common/SaveApis.Core.Common.csproj", "src/SaveApis.Core.Common/"]
COPY ["src/SaveApis.Core.Generator/SaveApis.Core.Generator.csproj", "src/SaveApis.Core.Generator/"]
RUN dotnet restore "examples/Example.Web/Example.Web.csproj"
COPY . .
WORKDIR "/src/examples/Example.Web"
RUN dotnet build "Example.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Example.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Example.Web.dll"]
