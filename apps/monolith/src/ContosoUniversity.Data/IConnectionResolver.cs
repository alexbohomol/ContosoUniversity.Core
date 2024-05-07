namespace ContosoUniversity.Data;

using Microsoft.Data.SqlClient;

public interface IConnectionResolver
{
    SqlConnectionStringBuilder CreateFor(string connectionStringName);
}
