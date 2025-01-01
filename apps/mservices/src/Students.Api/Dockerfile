FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Students.Api/Students.Api.csproj", "src/Students.Api/"]
COPY ["src/ContosoUniversity.SharedKernel/ContosoUniversity.SharedKernel.csproj", "src/ContosoUniversity.SharedKernel/"]
COPY ["src/ContosoUniversity.Messaging.Contracts/ContosoUniversity.Messaging.Contracts.csproj", "src/ContosoUniversity.Messaging.Contracts/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/Students.Data.Writes/Students.Data.Writes.csproj", "src/Students.Data.Writes/"]
COPY ["src/Students.Core/Students.Core.csproj", "src/Students.Core/"]
COPY ["src/Students.Data.Reads/Students.Data.Reads.csproj", "src/Students.Data.Reads/"]
RUN dotnet restore "src/Students.Api/Students.Api.csproj"
COPY . .
WORKDIR "/src/src/Students.Api"
RUN dotnet build "Students.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Students.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Students.Api.dll"]
