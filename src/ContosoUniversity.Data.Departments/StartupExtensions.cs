namespace ContosoUniversity.Data.Departments
{
    using Domain.Contracts;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        public static void AddDepartmentsDataLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DepartmentsContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            
            services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        }
    }
}