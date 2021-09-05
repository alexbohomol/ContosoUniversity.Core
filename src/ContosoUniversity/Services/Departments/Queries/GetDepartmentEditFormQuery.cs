namespace ContosoUniversity.Services.Departments.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using ViewModels;
    using ViewModels.Departments;

    public record GetDepartmentEditFormQuery(Guid Id) : IRequest<EditDepartmentForm>;
    
    public class GetDepartmentEditFormQueryHandler : IRequestHandler<GetDepartmentEditFormQuery, EditDepartmentForm>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public GetDepartmentEditFormQueryHandler(
            IInstructorsRepository instructorsRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _instructorsRepository = instructorsRepository;
            _departmentsRepository = departmentsRepository;
        }
        
        public async Task<EditDepartmentForm> Handle(GetDepartmentEditFormQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.Id, cancellationToken);

            var instructorNames = await _instructorsRepository.GetInstructorNamesReference(cancellationToken);

            return department == null
                ? null
                : new EditDepartmentForm
                {
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    AdministratorId = department.AdministratorId,
                    ExternalId = department.ExternalId,
                    // RowVersion = department.RowVersion,
                    InstructorsDropDown = instructorNames.ToSelectList(department.AdministratorId ?? default)
                };
        }
    }
}