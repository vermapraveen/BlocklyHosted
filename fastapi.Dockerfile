FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["FastApi/*.csproj", "FastApi/"]
COPY ["Common/*.csproj", "Common/"]

COPY . .
WORKDIR "/src/FastApi"
RUN dotnet restore

RUN dotnet build "FastApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FastApi.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastApi.dll"]