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

    public class DepartmentEditFormQueryHandler : IRequestHandler<DepartmentEditFormQuery, DepartmentEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;

        public DepartmentEditFormQueryHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        public async Task<DepartmentEditForm> Handle(DepartmentEditFormQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken);

            return department == null
                ? null
                : new DepartmentEditForm
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    InstructorId = department.Administrator?.ExternalId,
                    ExternalId = department.ExternalId,
                    RowVersion = department.RowVersion,
                    InstructorsDropDown = (await _departmentsContext.GetInstructorsNames()).ToSelectList(department.Administrator?.ExternalId ?? default)
                };
        }
    }
}