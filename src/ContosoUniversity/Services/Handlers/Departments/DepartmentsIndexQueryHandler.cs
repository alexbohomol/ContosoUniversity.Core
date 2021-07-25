namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Queries.Departments;

    using ViewModels.Departments;

    public class DepartmentsIndexQueryHandler : IRequestHandler<DepartmentsIndexQuery, IList<DepartmentListItemViewModel>>
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentsIndexQueryHandler(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<IList<DepartmentListItemViewModel>> Handle(DepartmentsIndexQuery request, CancellationToken cancellationToken)
        {
            var departments = await _departmentsRepository.GetAll();
            
            return departments.Select(x => new DepartmentListItemViewModel
            {
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                Administrator = x.Administrator?.FullName,
                ExternalId = x.EntityId
            }).ToList();
        }
    }
}