﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj", "src/ContosoUniversity.Mvc/"]
COPY ["src/ContosoUniversity.SharedKernel/ContosoUniversity.SharedKernel.csproj", "src/ContosoUniversity.SharedKernel/"]
COPY ["src/ContosoUniversity.Messaging.Contracts/ContosoUniversity.Messaging.Contracts.csproj", "src/ContosoUniversity.Messaging.Contracts/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/Courses.Core/Courses.Core.csproj", "src/Courses.Core/"]
COPY ["src/Students.Data.Reads/Students.Data.Reads.csproj", "src/Students.Data.Reads/"]
COPY ["src/Students.Core/Students.Core.csproj", "src/Students.Core/"]
COPY ["src/Students.Data.Writes/Students.Data.Writes.csproj", "src/Students.Data.Writes/"]
COPY ["src/ContosoUniversity.Application/ContosoUniversity.Application.csproj", "src/ContosoUniversity.Application/"]
COPY ["src/Departments.Core/Departments.Core.csproj", "src/Departments.Core/"]
COPY ["src/Departments.Data.Reads/Departments.Data.Reads.csproj", "src/Departments.Data.Reads/"]
COPY ["src/Departments.Data.Writes/Departments.Data.Writes.csproj", "src/Departments.Data.Writes/"]
COPY ["src/ContosoUniversity.ApiClients/ContosoUniversity.ApiClients.csproj", "src/ContosoUniversity.ApiClients/"]
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
