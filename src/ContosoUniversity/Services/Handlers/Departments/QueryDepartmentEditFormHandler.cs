namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Departments;

    using ViewModels;
    using ViewModels.Departments;

    public class QueryDepartmentEditFormHandler : IRequestHandler<QueryDepartmentEditForm, DepartmentEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;

        public QueryDepartmentEditFormHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        public async Task<DepartmentEditForm> Handle(QueryDepartmentEditForm request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken: cancellationToken);

            return department == null
                ? null
                : new DepartmentEditForm
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    InstructorId = department.InstructorId,
                    ExternalId = department.ExternalId,
                    RowVersion = department.RowVersion,
                    InstructorsDropDown = (await _departmentsContext.GetInstructorsNames()).ToSelectList(department.InstructorId.GetValueOrDefault())
                };
        }
    }
}