using Course = Courses.Core.Domain.Course;

namespace Courses.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class EditCourseCommandHandler(
    ICoursesRwRepository coursesRepository)
    : IRequestHandler<EditCourseCommand>
{
    public async Task Handle(EditCourseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Course course = await coursesRepository.GetById(request.Id, cancellationToken);

        ArgumentNullException.ThrowIfNull(course);

        course.Update(
            request.Title,
            request.Credits,
            request.DepartmentId);

        await coursesRepository.Save(course, cancellationToken);
    }
}
