### Contoso University

![.NET Core](https://github.com/alexbogomol/ContosoUniversity.Core/workflows/.NET%20Core/badge.svg)

Walkthrough the canonical [tutorial](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc) updated to .Net Core version is [here](https://docs.microsoft.com/uk-ua/aspnet/core/data/ef-mvc).

.Net Core implementation imported from [cu-final](https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/data/ef-mvc/intro/samples/cu-final).

#### Switch to SQL Server on Linux container
* [Install and connect container](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker)
* [Configure Docker container](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker)
```
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>' \
    --name sqlserver \
    -p 1433:1433 \
    -v sqlvolume:/var/opt/mssql \
    -d mcr.microsoft.com/mssql/server
```
