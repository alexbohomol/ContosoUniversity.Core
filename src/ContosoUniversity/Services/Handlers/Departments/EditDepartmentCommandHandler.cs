namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class EditDepartmentCommandHandler : AsyncRequestHandler<DepartmentEditCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public EditDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(DepartmentEditCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.ExternalId == request.ExternalId, cancellationToken);

            department.Name = request.Name;
            department.StartDate = request.StartDate;
            department.Budget = request.Budget;
            department.InstructorId = request.InstructorId;
            department.RowVersion = request.RowVersion;

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}