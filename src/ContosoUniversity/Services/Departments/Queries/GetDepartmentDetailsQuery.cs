namespace ContosoUniversity.Services.Departments.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Instructor;

    using MediatR;

    using ViewModels.Departments;

    public record GetDepartmentDetailsQuery(Guid Id) : IRequest<DepartmentDetailsViewModel>;
    
    public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetailsViewModel>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public GetDepartmentDetailsQueryHandler(
            IInstructorsRepository instructorsRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _instructorsRepository = instructorsRepository;
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<DepartmentDetailsViewModel> Handle(GetDepartmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id, cancellationToken);
            if (department is null)
                throw new EntityNotFoundException(nameof(department), request.Id);

            var fullname = string.Empty;
            if (department.AdministratorId.HasValue)
            {
                var administrator = await _instructorsRepository.GetById(
                    department.AdministratorId.Value,
                    cancellationToken);
                
                if (administrator is null)
                    throw new EntityNotFoundException(nameof(administrator), department.AdministratorId.Value);
                fullname = administrator.FullName();
            }

            return new DepartmentDetailsViewModel
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = fullname,
                ExternalId = department.EntityId
            };
        }
    }
}