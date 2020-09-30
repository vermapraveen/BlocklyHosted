FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Common/*.csproj ./Common/
COPY Models/*.csproj ./Models/
COPY CodeGenerator/*.csproj ./CodeGenerator/
COPY Tests/*.csproj ./Tests/
COPY BlkHost/*.csproj ./BlkHost/

RUN dotnet restore
COPY . .

WORKDIR /src/Common
RUN dotnet build -c Release -o /app

WORKDIR /src/Models
RUN dotnet build -c Release -o /app

WORKDIR /src/CodeGenerator
RUN dotnet build -c Release -o /app

WORKDIR /src/Tests
RUN dotnet build -c Release -o /app

WORKDIR /src/BlkHost
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BlkHost.dll"]