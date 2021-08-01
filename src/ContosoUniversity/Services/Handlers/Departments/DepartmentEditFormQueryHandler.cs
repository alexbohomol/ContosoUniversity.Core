namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Queries.Departments;

    using ViewModels;
    using ViewModels.Departments;

    public class DepartmentEditFormQueryHandler : IRequestHandler<DepartmentEditFormQuery, DepartmentEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentEditFormQueryHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<DepartmentEditForm> Handle(DepartmentEditFormQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id);

            return department == null
                ? null
                : new DepartmentEditForm
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    AdministratorId = department.AdministratorId,
                    ExternalId = department.EntityId,
                    // RowVersion = department.RowVersion,
                    InstructorsDropDown = (await _departmentsContext.GetInstructorsNames()).ToSelectList(department.AdministratorId ?? default)
                };
        }
    }
}