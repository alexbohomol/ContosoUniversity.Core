namespace ContosoUniversity.Services.Handlers.Departments
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;
    using Data.Departments.Models;

    using MediatR;

    using ViewModels.Departments;

    public class CreateDepartmentCommandHandler : AsyncRequestHandler<DepartmentCreateForm>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CreateDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(DepartmentCreateForm request, CancellationToken cancellationToken)
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