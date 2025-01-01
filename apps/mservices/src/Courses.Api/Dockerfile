FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Courses.Api/Courses.Api.csproj", "src/Courses.Api/"]
COPY ["src/ContosoUniversity.SharedKernel/ContosoUniversity.SharedKernel.csproj", "src/ContosoUniversity.SharedKernel/"]
COPY ["src/ContosoUniversity.Messaging.Contracts/ContosoUniversity.Messaging.Contracts.csproj", "src/ContosoUniversity.Messaging.Contracts/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/Courses.Core/Courses.Core.csproj", "src/Courses.Core/"]
COPY ["src/Courses.Data.Reads/Courses.Data.Reads.csproj", "src/Courses.Data.Reads/"]
COPY ["src/Courses.Data.Writes/Courses.Data.Writes.csproj", "src/Courses.Data.Writes/"]
RUN dotnet restore "src/Courses.Api/Courses.Api.csproj"
COPY . .
WORKDIR "/src/src/Courses.Api"
RUN dotnet build "Courses.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Courses.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.Api.dll"]
