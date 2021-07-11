namespace ContosoUniversity.Services.Handlers.Departments
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;
    using Data.Departments.Models;

    using MediatR;

    public class CreateDepartmentCommandHandler : AsyncRequestHandler<CreateDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CreateDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            _departmentsContext.Add(new Department
            {
                Name = request.Name,
                Budget = request.Budget,
                StartDate = request.StartDate,
                InstructorId = request.InstructorId,
                ExternalId = Guid.NewGuid()
            });
            
            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}