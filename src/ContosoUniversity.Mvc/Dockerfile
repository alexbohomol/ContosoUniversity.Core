FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ContosoUniversity.Application/ContosoUniversity.Application.csproj", "src/ContosoUniversity.Application/"]
COPY ["src/ContosoUniversity.Data.Courses.Reads/ContosoUniversity.Data.Courses.Reads.csproj", "src/ContosoUniversity.Data.Courses.Reads/"]
COPY ["src/ContosoUniversity.Data.Courses.Writes/ContosoUniversity.Data.Courses.Writes.csproj", "src/ContosoUniversity.Data.Courses.Writes/"]
COPY ["src/ContosoUniversity.Data.Departments.Reads/ContosoUniversity.Data.Departments.Reads.csproj", "src/ContosoUniversity.Data.Departments.Reads/"]
COPY ["src/ContosoUniversity.Data.Departments.Writes/ContosoUniversity.Data.Departments.Writes.csproj", "src/ContosoUniversity.Data.Departments.Writes/"]
COPY ["src/ContosoUniversity.Data.Students.Reads/ContosoUniversity.Data.Students.Reads.csproj", "src/ContosoUniversity.Data.Students.Reads/"]
COPY ["src/ContosoUniversity.Data.Students.Writes/ContosoUniversity.Data.Students.Writes.csproj", "src/ContosoUniversity.Data.Students.Writes/"]
COPY ["src/ContosoUniversity.Data/ContosoUniversity.Data.csproj", "src/ContosoUniversity.Data/"]
COPY ["src/ContosoUniversity.Domain/ContosoUniversity.Domain.csproj", "src/ContosoUniversity.Domain/"]
COPY ["src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj", "src/ContosoUniversity.Mvc/"]
RUN dotnet restore "src/ContosoUniversity.Mvc/ContosoUniversity.Mvc.csproj"
COPY . .
WORKDIR "/src/src/ContosoUniversity.Mvc"
RUN dotnet build "ContosoUniversity.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContosoUniversity.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ContosoUniversity.Mvc.dll"]
