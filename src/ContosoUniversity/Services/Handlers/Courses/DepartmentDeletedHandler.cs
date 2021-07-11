namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using Events;

    using MediatR;

    public class DepartmentDeletedHandler : INotificationHandler<DepartmentDeleted>
    {
        private readonly ICoursesRepository _coursesRepository;

        public DepartmentDeletedHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        
        public async Task Handle(DepartmentDeleted notification, CancellationToken cancellationToken)
        {
            await _coursesRepository.Remove(notification.CourseIds);
        }
    }
}