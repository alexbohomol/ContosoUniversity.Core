namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Course;

using MediatR;

public class CreateCourseCommand : IRequest
{
    public int CourseCode { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public Guid DepartmentId { get; set; }
}

internal class CreateCourseCommandHandler(ICoursesRwRepository coursesRepository) : IRequestHandler<CreateCourseCommand>
{
    public async Task Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        await coursesRepository.Save(
            Course.Create(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId),
            cancellationToken);
    }
}
