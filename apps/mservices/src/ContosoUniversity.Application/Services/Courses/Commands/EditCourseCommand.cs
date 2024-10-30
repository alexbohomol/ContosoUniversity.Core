namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Courses.Core;
using global::Courses.Core.Domain;

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
