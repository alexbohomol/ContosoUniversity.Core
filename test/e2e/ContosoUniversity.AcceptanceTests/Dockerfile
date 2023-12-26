FROM mcr.microsoft.com/playwright/dotnet:v1.33.0-focal AS base
WORKDIR /src
COPY ["test/e2e/ContosoUniversity.AcceptanceTests", "ContosoUniversity.AcceptanceTests"]
WORKDIR /src/ContosoUniversity.AcceptanceTests
RUN dotnet restore
RUN dotnet build -c Release --no-restore
