namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Departments;

    public class EditDepartmentCommandHandler : AsyncRequestHandler<DepartmentEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;

        public EditDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(DepartmentEditForm request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.ExternalId == request.ExternalId, cancellationToken: cancellationToken);

            department.Name = request.Name;
            department.StartDate = request.StartDate;
            department.Budget = request.Budget;
            department.InstructorId = request.InstructorId;
            department.RowVersion = request.RowVersion;

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}