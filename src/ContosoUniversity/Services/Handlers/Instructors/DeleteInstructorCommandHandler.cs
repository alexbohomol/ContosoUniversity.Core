namespace ContosoUniversity.Services.Handlers.Instructors
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Instructors;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Department;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteInstructorCommandHandler : AsyncRequestHandler<DeleteInstructorCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public DeleteInstructorCommandHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ExternalId == request.Id, cancellationToken);

            Department[] administratedDepartments = await _departmentsRepository.GetByAdministrator(instructor.ExternalId);
            foreach (var department in administratedDepartments)
            {
                department.DisassociateAdministrator();
                await _departmentsRepository.Save(department);
            }
            
            _departmentsContext.Instructors.Remove(instructor);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}