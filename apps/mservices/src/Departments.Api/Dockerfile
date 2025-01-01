FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Departments.Api/Departments.Api.csproj", "src/Departments.Api/"]
COPY ["src/Departments.Data.Writes/Departments.Data.Writes.csproj", "src/Departments.Data.Writes/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/ContosoUniversity.SharedKernel/ContosoUniversity.SharedKernel.csproj", "src/ContosoUniversity.SharedKernel/"]
COPY ["src/Departments.Core/Departments.Core.csproj", "src/Departments.Core/"]
COPY ["src/ContosoUniversity.Messaging.Contracts/ContosoUniversity.Messaging.Contracts.csproj", "src/ContosoUniversity.Messaging.Contracts/"]
COPY ["src/Departments.Data.Reads/Departments.Data.Reads.csproj", "src/Departments.Data.Reads/"]
RUN dotnet restore "src/Departments.Api/Departments.Api.csproj"
COPY . .
WORKDIR "/src/src/Departments.Api"
RUN dotnet build "Departments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Departments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Departments.Api.dll"]
