namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

public class EditCourseCommand : IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public Guid DepartmentId { get; set; }
}

internal class EditCourseCommandHandler(ICoursesRwRepository coursesRepository)
    : IRequestHandler<EditCourseCommand>
{
    public async Task Handle(EditCourseCommand request, CancellationToken cancellationToken)
    {
        Domain.Course.Course course = await coursesRepository.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new EntityNotFoundException(nameof(course), request.Id);
        }

        course.Update(
            request.Title,
            request.Credits,
            request.DepartmentId);

        await coursesRepository.Save(course, cancellationToken);
    }
}
