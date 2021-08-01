namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

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
            
            if (request.InstructorId.HasValue)
            {
                var instructor = await _departmentsContext.Instructors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ExternalId == request.InstructorId);

               if (instructor is null)
                   throw new EntityNotFoundException(nameof(instructor), request.InstructorId.Value);
                
                department.AssociateAdministrator(request.InstructorId.Value);
            }
            else
            {
                department.DisassociateAdministrator();
            }

            await _departmentsRepository.Save(department);
        }
    }
}