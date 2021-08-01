namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

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

            var instructorsNames = await _departmentsContext.GetInstructorsNames();

            return departments.Select(x => new DepartmentListItemViewModel
            {
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                Administrator = x.AdministratorId.HasValue
                    ? instructorsNames.ContainsKey(x.AdministratorId.Value) 
                        ? instructorsNames[x.AdministratorId.Value] 
                        : string.Empty 
                    : string.Empty,
                ExternalId = x.EntityId
            }).ToList();
        }
    }
}