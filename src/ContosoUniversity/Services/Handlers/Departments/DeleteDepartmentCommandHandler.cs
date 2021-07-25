namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Notifications;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteDepartmentCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMediator _mediator;

        public DeleteDepartmentCommandHandler(
            DepartmentsContext departmentsContext,
            IDepartmentsRepository departmentsRepository,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }
        
        protected override async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var exists = await _departmentsRepository.Exists(request.Id);
            if (!exists)
                throw new EntityNotFoundException(nameof(exists), request.Id);
            
            var relatedCourses = await _coursesRepository.GetByDepartmentId(request.Id);
            var relatedCoursesIds = relatedCourses.Select(x => x.EntityId).ToArray();

            /*
             * remove related assignments
             */
            var relatedAssignments = await _departmentsContext.CourseAssignments
                .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                .ToArrayAsync(cancellationToken: cancellationToken);
            _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
            
            await _departmentsRepository.Remove(request.Id);
            
            /*
             * - remove related courses
             * - withdraw enrolled students
             * - remove related assignments ???
             */
            await _mediator.Publish(
                new DepartmentDeletedNotification(request.Id, relatedCoursesIds),
                cancellationToken);
        }
    }
}