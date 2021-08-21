namespace ContosoUniversity.Services.Departments.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using ViewModels;
    using ViewModels.Departments;

    public record GetDepartmentEditFormQuery(Guid Id) : IRequest<DepartmentEditForm>;
    
    public class GetDepartmentEditFormQueryHandler : IRequestHandler<GetDepartmentEditFormQuery, DepartmentEditForm>
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly IInstructorsRepository _instructorsRepository;

        public GetDepartmentEditFormQueryHandler(
            IDepartmentsRepository departmentsRepository,
            IInstructorsRepository instructorsRepository)
        {
            _departmentsRepository = departmentsRepository;
            _instructorsRepository = instructorsRepository;
        }
        
        public async Task<DepartmentEditForm> Handle(GetDepartmentEditFormQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id);

            var instructorNames = await _instructorsRepository.GetInstructorNamesReference();

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
                    InstructorsDropDown = instructorNames.ToSelectList(department.AdministratorId ?? default)
                };
        }
    }
}