FROM mcr.microsoft.com/playwright/dotnet:v1.33.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test/e2e/ContosoUniversity.AcceptanceTests/ContosoUniversity.AcceptanceTests.csproj", "ContosoUniversity.AcceptanceTests/"]
RUN dotnet restore "ContosoUniversity.AcceptanceTests/ContosoUniversity.AcceptanceTests.csproj"

COPY ["test/e2e/ContosoUniversity.AcceptanceTests", "ContosoUniversity.AcceptanceTests"]

COPY . .
WORKDIR "/src/ContosoUniversity.AcceptanceTests"
RUN dotnet build "ContosoUniversity.AcceptanceTests.csproj" -c Release -o /tests
WORKDIR "/tests"