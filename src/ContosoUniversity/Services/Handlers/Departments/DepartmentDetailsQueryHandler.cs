namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Departments;

    using ViewModels.Departments;

    public class DepartmentDetailsQueryHandler : IRequestHandler<DepartmentDetailsQuery, DepartmentDetailsViewModel>
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentDetailsQueryHandler(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<DepartmentDetailsViewModel> Handle(DepartmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id);
            if (department is null)
                throw new EntityNotFoundException(nameof(department), request.Id);
            
            return new DepartmentDetailsViewModel
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = department.Administrator?.FullName,
                ExternalId = department.EntityId
            };
        }
    }
}