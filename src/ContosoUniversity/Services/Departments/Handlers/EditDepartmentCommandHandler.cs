namespace ContosoUniversity.Services.Departments.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class EditDepartmentCommandHandler : AsyncRequestHandler<EditDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public EditDepartmentCommandHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.ExternalId);

            department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);
            
            if (request.AdministratorId.HasValue)
            {
                var instructor = await _departmentsContext.Instructors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ExternalId == request.AdministratorId);

               if (instructor is null)
                   throw new EntityNotFoundException(nameof(instructor), request.AdministratorId.Value);
                
                department.AssociateAdministrator(request.AdministratorId.Value);
            }
            else
            {
                department.DisassociateAdministrator();
            }

            await _departmentsRepository.Save(department);
        }
    }
}