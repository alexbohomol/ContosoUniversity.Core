namespace ContosoUniversity.Services.Departments.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries;

    using ViewModels.Departments;

    public class DepartmentDetailsQueryHandler : IRequestHandler<DepartmentDetailsQuery, DepartmentDetailsViewModel>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentDetailsQueryHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<DepartmentDetailsViewModel> Handle(DepartmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id);
            if (department is null)
                throw new EntityNotFoundException(nameof(department), request.Id);

            var fullname = string.Empty;
            if (department.AdministratorId.HasValue)
            {
                var administrator = await _departmentsContext.Instructors.FirstOrDefaultAsync(x => x.ExternalId == department.AdministratorId);
                if (administrator is null)
                    throw new EntityNotFoundException(nameof(administrator), department.AdministratorId.Value);
                fullname = administrator.FullName;
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