namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Department;

    using MediatR;

    public record DeleteInstructorCommand(Guid Id) : IRequest;
    
    public class DeleteInstructorCommandHandler : AsyncRequestHandler<DeleteInstructorCommand>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public DeleteInstructorCommandHandler(
            IInstructorsRepository instructorsRepository, 
            IDepartmentsRepository departmentsRepository)
        {
            _instructorsRepository = instructorsRepository;
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            if (!await _instructorsRepository.Exists(request.Id))
                throw new EntityNotFoundException("instructor", request.Id);

            Department[] administratedDepartments = await _departmentsRepository.GetByAdministrator(request.Id);
            foreach (var department in administratedDepartments)
            {
                department.DisassociateAdministrator();
                await _departmentsRepository.Save(department);
            }
            
            await _instructorsRepository.Remove(request.Id);
        }
    }
}