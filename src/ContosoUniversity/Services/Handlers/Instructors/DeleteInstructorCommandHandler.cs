namespace ContosoUniversity.Services.Handlers.Instructors
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Instructors;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteInstructorCommandHandler : AsyncRequestHandler<DeleteInstructorCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public DeleteInstructorCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ExternalId == request.Id, cancellationToken);

            var departments = await _departmentsContext.Departments
                .Where(d => d.InstructorId == instructor.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            departments.ForEach(d => d.InstructorId = null);

            _departmentsContext.Instructors.Remove(instructor);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}