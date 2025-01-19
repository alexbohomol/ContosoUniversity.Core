namespace Courses.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class CreateCourseCommandHandler(
    ICoursesRwRepository repository)
    : IRequestHandler<CreateCourseCommand, Course>
{
    public async Task<Course> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var course = Course.Create(
            request.CourseCode,
            request.Title,
            request.Credits,
            request.DepartmentId);

        await repository.Save(course, cancellationToken);

        return course;
    }
}
