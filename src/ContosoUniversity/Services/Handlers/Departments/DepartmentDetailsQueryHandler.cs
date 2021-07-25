namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Departments;

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
            
            /*
             * This query will become excessive after introducing 'Instructor'
             * value object as a part of 'Department' root aggregate
             */
            var instructor = await _departmentsContext.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.ExternalId == department.AdministratorId, 
                    cancellationToken);

            return department == null
                ? null
                : new DepartmentDetailsViewModel
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    Administrator = instructor?.FullName ?? string.Empty,
                    ExternalId = department.EntityId
                };
        }
    }
}