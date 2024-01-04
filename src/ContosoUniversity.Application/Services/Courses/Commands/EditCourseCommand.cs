namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly.Projections;
using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

public class EditCourseCommand : IRequest
{
    public EditCourseCommand(Course course)
    {
        Id = course.ExternalId;
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
    private readonly ICoursesRwRepository _coursesRepository;

    public EditCourseCommandHandler(ICoursesRwRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    protected override async Task Handle(EditCourseCommand request, CancellationToken cancellationToken)
    {
        Domain.Course.Course course = await _coursesRepository.GetById(request.Id, cancellationToken);
        if (course == null)
        {
            throw new EntityNotFoundException(nameof(course), request.Id);
        }

        course.Update(
            request.Title,
            request.Credits,
            request.DepartmentId);

        await _coursesRepository.Save(course, cancellationToken);
    }
}
