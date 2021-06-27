namespace ContosoUniversity.Services.Handlers.Students
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using Events;

    using MediatR;

    public class CourseDeletedHandler : INotificationHandler<CourseDeleted>
    {
        private readonly IStudentsRepository _studentsRepository;

        public CourseDeletedHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task Handle(CourseDeleted notification, CancellationToken cancellationToken)
        {
            Guid[] courseIds = { notification.Id };
            
            var enrolledStudents = await _studentsRepository.GetStudentsEnrolledForCourses(courseIds);
            if (enrolledStudents.Any())
            {
                foreach (var student in enrolledStudents)
                {
                    student.WithdrawCourses(courseIds);
                    await _studentsRepository.Save(student);
                }
            }
        }
    }
}