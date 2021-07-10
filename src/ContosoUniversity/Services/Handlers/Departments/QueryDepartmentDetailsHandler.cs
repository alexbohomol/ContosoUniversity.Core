namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Departments;

    using ViewModels.Departments;

    public class QueryDepartmentDetailsHandler : IRequestHandler<QueryDepartmentDetails, DepartmentDetailsViewModel>
    {
        private readonly DepartmentsContext _departmentsContext;

        public QueryDepartmentDetailsHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        public async Task<DepartmentDetailsViewModel> Handle(QueryDepartmentDetails request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .FromSqlInterpolated($"SELECT * FROM [dpt].Department WHERE ExternalId = {request.Id}")
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return department == null
                ? null
                : new DepartmentDetailsViewModel
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    Administrator = department.Administrator?.FullName,
                    ExternalId = department.ExternalId
                };
        }
    }
}