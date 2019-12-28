### Contoso University

Walkthrough the canonical [tutorial](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc)

#### Notes on Module-1

##### Issue: Connection string (as of VS2015)

There was a problem when try to copy and use the connection string (versioned) from original samples. Issue caused by the new way we should treat (version-independent) connection strings as of Visual Studio 2015 (RC).

Connection string updated to switch from LocalDb to SQL Server.

Before starting the first time run **`PM> Update-Database`** in Package Manager Console.

**Solution**:

just replace **`Data Source=(LocalDb)\v11.0;...`**

with **`Data Source=(LocalDb)\MSSQLLocalDB;...`**.

**References**:
* [Bill Wagner post on this](http://thebillwagner.com/Blog/Item/VersionindependentlocalDBinVisualStudio2015%5E1654)
* [StackOverFlow question](http://stackoverflow.com/questions/21563940/how-to-connect-to-localdb-in-visual-studio-server-explorer)
