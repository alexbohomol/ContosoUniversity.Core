using Course = Courses.Core.Domain.Course;

namespace Courses.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class CreateCourseCommandHandler(
    ICoursesRwRepository repository)
    : IRequestHandler<CreateCourseCommand>
{
    public async Task Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await repository.Save(
            Course.Create(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId),
            cancellationToken);
    }
}
