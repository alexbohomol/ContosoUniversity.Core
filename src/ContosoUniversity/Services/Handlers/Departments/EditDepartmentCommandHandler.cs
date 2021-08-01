namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;

    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class EditDepartmentCommandHandler : AsyncRequestHandler<EditDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public EditDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.ExternalId == request.ExternalId, cancellationToken);

            department.Name = request.Name;
            department.StartDate = request.StartDate;
            department.Budget = request.Budget;
            department.RowVersion = request.RowVersion;

            if (request.InstructorId.HasValue)
            {
                var instructor = await _departmentsContext.Instructors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ExternalId == request.InstructorId);

                department.InstructorId = instructor?.Id ?? throw new EntityNotFoundException(nameof(instructor), request.InstructorId.Value);
            }
            else
            {
                department.InstructorId = null;
            }

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}