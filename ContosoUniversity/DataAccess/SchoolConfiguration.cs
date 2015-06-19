using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ContosoUniversity.DataAccess
{
    public class SchoolConfiguration : DbConfiguration
    {
        public SchoolConfiguration()
        {
            SetExecutionStrategy(
                "System.Data.SqlClient", 
                () => new SqlAzureExecutionStrategy());

            AddInterceptor(new SchoolInterceptorTransientErrors());
            AddInterceptor(new SchoolInterceptorLogging());

            //--> 'register-anywhere' alternative
            //DbInterception.Add(new SchoolInterceptorTransientErrors());
            //DbInterception.Add(new SchoolInterceptorLogging());
        }
    }
}