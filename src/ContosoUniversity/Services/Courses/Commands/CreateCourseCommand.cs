namespace ContosoUniversity.Services.Courses.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Course;

    using MediatR;

    public class CreateCourseCommand : IRequest
    {
        public int CourseCode { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
    
    public class CreateCourseCommandHandler : AsyncRequestHandler<CreateCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public CreateCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        protected override Task Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            return _coursesRepository.Save(
                new Course(
                    request.CourseCode,
                    request.Title,
                    request.Credits,
                    request.DepartmentId),
                cancellationToken);
        }
    }
}