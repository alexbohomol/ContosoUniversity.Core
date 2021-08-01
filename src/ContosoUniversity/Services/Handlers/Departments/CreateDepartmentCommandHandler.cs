namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Domain.Contracts;
    using Domain.Department;

    using MediatR;

    public class CreateDepartmentCommandHandler : AsyncRequestHandler<CreateDepartmentCommand>
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public CreateDepartmentCommandHandler(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = new Department(request.Name, request.Budget, request.StartDate, request.AdministratorId);

            await _departmentsRepository.Save(department);
        }
    }
}