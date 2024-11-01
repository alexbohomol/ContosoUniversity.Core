using Course = Courses.Core.Domain.Course;
using ICoursesRwRepository = Courses.Core.ICoursesRwRepository;

namespace ContosoUniversity.Application.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

public record EditCourseCommand(
    Guid Id,
    string Title,
    int Credits,
    Guid DepartmentId) : IRequest;

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
