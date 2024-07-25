FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FugleGrabber/FugleGrabber.csproj", "FugleGrabber/"]
RUN dotnet restore "FugleGrabber/FugleGrabber.csproj"
COPY . .
WORKDIR "/src/FugleGrabber"
RUN dotnet build "FugleGrabber.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FugleGrabber.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FugleGrabber.dll"]
