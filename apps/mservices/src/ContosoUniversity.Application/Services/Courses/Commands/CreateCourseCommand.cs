namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Courses.Core;
using global::Courses.Core.Domain;

using MediatR;

public record CreateCourseCommand(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId) : IRequest;

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
