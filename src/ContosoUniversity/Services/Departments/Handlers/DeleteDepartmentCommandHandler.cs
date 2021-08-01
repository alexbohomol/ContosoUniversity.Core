namespace ContosoUniversity.Services.Departments.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Notifications;

    public class DeleteDepartmentCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMediator _mediator;

        public DeleteDepartmentCommandHandler(
            IDepartmentsRepository departmentsRepository,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsRepository = departmentsRepository;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }
        
        protected override async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var exists = await _departmentsRepository.Exists(request.Id);
            if (!exists)
                throw new EntityNotFoundException(nameof(exists), request.Id);
            
            await _departmentsRepository.Remove(request.Id);

            var relatedCoursesIds = (await _coursesRepository.GetByDepartmentId(request.Id))
                .Select(x => x.EntityId)
                .ToArray();
            
            /*
             * - remove related courses
             * - withdraw enrolled students
             * - remove related assignments (restrain assigned instructors)
             */
            if (relatedCoursesIds.Any())
            {
                await _mediator.Publish(
                    new DepartmentDeletedNotification(relatedCoursesIds),
                    cancellationToken);
            }
        }
    }
}