namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Events;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteDepartmentCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMediator _mediator;

        public DeleteDepartmentCommandHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }
        
        protected override async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .FirstOrDefaultAsync(x => x.ExternalId == request.Id, cancellationToken: cancellationToken);
            if (department == null)
                throw new EntityNotFoundException(nameof(department), request.Id);
            
            var relatedCourses = await _coursesRepository.GetByDepartmentId(request.Id);
            var relatedCoursesIds = relatedCourses.Select(x => x.EntityId).ToArray();

            /*
             * remove related assignments
             */
            var relatedAssignments = await _departmentsContext.CourseAssignments
                .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                .ToArrayAsync(cancellationToken: cancellationToken);
            _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);
            
            /*
             * remove department
             */
            _departmentsContext.Departments.Remove(department);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
            
            /*
             * remove related courses and withdraw enrolled students
             */
            await _mediator.Publish(
                new DepartmentDeleted(request.Id, relatedCoursesIds),
                cancellationToken);
        }
    }
}