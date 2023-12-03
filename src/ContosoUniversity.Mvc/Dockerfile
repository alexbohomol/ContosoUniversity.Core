FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj", "src/ContosoUniversity.Mvc/"]
COPY ["src/ContosoUniversity.Data.Courses.Reads/ContosoUniversity.Data.Courses.Reads.csproj", "src/ContosoUniversity.Data.Courses.Reads/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/ContosoUniversity.Application/ContosoUniversity.Application.csproj", "src/ContosoUniversity.Application/"]
COPY ["src/ContosoUniversity.Domain/ContosoUniversity.Domain.csproj", "src/ContosoUniversity.Domain/"]
COPY ["src/ContosoUniversity.Data.Courses.Writes/ContosoUniversity.Data.Courses.Writes.csproj", "src/ContosoUniversity.Data.Courses.Writes/"]
COPY ["src/ContosoUniversity.Data.Departments.Reads/ContosoUniversity.Data.Departments.Reads.csproj", "src/ContosoUniversity.Data.Departments.Reads/"]
COPY ["src/ContosoUniversity.Data.Departments.Writes/ContosoUniversity.Data.Departments.Writes.csproj", "src/ContosoUniversity.Data.Departments.Writes/"]
COPY ["src/ContosoUniversity.Data.Students.Reads/ContosoUniversity.Data.Students.Reads.csproj", "src/ContosoUniversity.Data.Students.Reads/"]
COPY ["src/ContosoUniversity.Data.Students.Writes/ContosoUniversity.Data.Students.Writes.csproj", "src/ContosoUniversity.Data.Students.Writes/"]
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
ENTRYPOINT ["dotnet", "ContosoUniversity.Mvc.dll"]
