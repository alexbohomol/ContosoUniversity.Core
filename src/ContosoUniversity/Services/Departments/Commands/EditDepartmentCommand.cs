namespace ContosoUniversity.Services.Departments.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public class EditDepartmentCommand : IRequest
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public Guid? AdministratorId { get; set; }
        public Guid ExternalId { get; set; }
        public byte[] RowVersion { get; set; }
    }
    
    public class EditDepartmentCommandHandler : AsyncRequestHandler<EditDepartmentCommand>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public EditDepartmentCommandHandler(
            IInstructorsRepository instructorsRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _instructorsRepository = instructorsRepository;
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.ExternalId, cancellationToken);
            if (department is null)
                throw new EntityNotFoundException(nameof(department), request.ExternalId);

            department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);
            
            if (request.AdministratorId.HasValue)
            {
                if (!await _instructorsRepository.Exists(request.AdministratorId.Value, cancellationToken))
                    throw new EntityNotFoundException("instructor", request.AdministratorId.Value);
                
                department.AssociateAdministrator(request.AdministratorId.Value);
            }
            else
            {
                department.DisassociateAdministrator();
            }

            await _departmentsRepository.Save(department, cancellationToken);
        }
    }
}