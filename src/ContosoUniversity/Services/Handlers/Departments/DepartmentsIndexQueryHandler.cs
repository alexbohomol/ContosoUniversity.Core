namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Departments;

    using ViewModels.Departments;

    public class DepartmentsIndexQueryHandler : IRequestHandler<DepartmentsIndexQuery, IList<DepartmentListItemViewModel>>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentsIndexQueryHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<IList<DepartmentListItemViewModel>> Handle(DepartmentsIndexQuery request, CancellationToken cancellationToken)
        {
            var departments = await _departmentsRepository.GetAll();

            /*
             * This query will become excessive after introducing 'Instructor'
             * value object as a part of 'Department' root aggregate
             */
            var instructorsNames = await _departmentsContext.Instructors
                .AsNoTracking()
                .Where(x => departments.Select(x1 => x1.AdministratorId).Contains(x.ExternalId))
                .ToDictionaryAsync(
                    x => x.ExternalId,
                    x => x.FullName,
                    cancellationToken);

            return departments.Select(x => new DepartmentListItemViewModel
            {
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                Administrator = instructorsNames.ContainsKey(x.AdministratorId) ? instructorsNames[x.AdministratorId] : string.Empty,
                ExternalId = x.EntityId
            }).ToList();
        }
    }
}