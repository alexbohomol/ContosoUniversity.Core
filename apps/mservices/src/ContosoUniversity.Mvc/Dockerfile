FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj", "src/ContosoUniversity.Mvc/"]
COPY ["src/ContosoUniversity.SharedKernel/ContosoUniversity.SharedKernel.csproj", "src/ContosoUniversity.SharedKernel/"]
COPY ["src/ContosoUniversity.Messaging.Contracts/ContosoUniversity.Messaging.Contracts.csproj", "src/ContosoUniversity.Messaging.Contracts/"]
COPY ["src/ContosoUniversity.Application/ContosoUniversity.Application.csproj", "src/ContosoUniversity.Application/"]
COPY ["src/Courses.Core/Courses.Core.csproj", "src/Courses.Core/"]
COPY ["src/ContosoUniversity.ApiClients/ContosoUniversity.ApiClients.csproj", "src/ContosoUniversity.ApiClients/"]
COPY ["src/Departments.Core/Departments.Core.csproj", "src/Departments.Core/"]
COPY ["src/Students.Core/Students.Core.csproj", "src/Students.Core/"]
RUN dotnet restore "src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj"
COPY . .
WORKDIR "/src/src/ContosoUniversity.Mvc"
RUN dotnet build "ContosoUniversity.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContosoUniversity.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/ContosoUniversity.Mvc/cert.pfx /https/
ENTRYPOINT ["dotnet", "ContosoUniversity.Mvc.dll"]
