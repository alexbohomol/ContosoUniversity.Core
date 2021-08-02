namespace ContosoUniversity.Services.Courses.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Course;

    using MediatR;

    public class EditCourseCommand : IRequest
    {
        public EditCourseCommand(Course course)
        {
            Id = course.EntityId;
            Title = course.Title;
            Credits = course.Credits;
            DepartmentId = course.DepartmentId;
        }

        public EditCourseCommand()
        {
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
    
    public class EditCourseCommandHandler : AsyncRequestHandler<EditCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public EditCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        protected override async Task Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            course.Update(
                request.Title,
                request.Credits,
                request.DepartmentId);

            await _coursesRepository.Save(course);
        }
    }
}