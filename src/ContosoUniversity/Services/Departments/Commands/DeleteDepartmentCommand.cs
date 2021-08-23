namespace ContosoUniversity.Services.Departments.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Notifications;

    public record DeleteDepartmentCommand(Guid Id) : IRequest;
    
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
            var exists = await _departmentsRepository.Exists(request.Id, cancellationToken);
            if (!exists)
                throw new EntityNotFoundException(nameof(exists), request.Id);
            
            await _departmentsRepository.Remove(request.Id, cancellationToken);

            var relatedCoursesIds = (await _coursesRepository.GetByDepartmentId(request.Id, cancellationToken))
                                    .Select(x => x.ExternalId)
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