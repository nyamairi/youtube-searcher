FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["YouTubeSearcher/YouTubeSearcher.csproj", "YouTubeSearcher/"]
RUN dotnet restore "YouTubeSearcher/YouTubeSearcher.csproj"
COPY . .
WORKDIR "/src/YouTubeSearcher"
RUN dotnet build "YouTubeSearcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YouTubeSearcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YouTubeSearcher.dll"]
