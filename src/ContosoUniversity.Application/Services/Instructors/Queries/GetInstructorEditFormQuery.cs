namespace ContosoUniversity.Application.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetInstructorEditFormQuery(Guid Id) : IRequest<GetInstructorEditFormQueryResult>;

public record GetInstructorEditFormQueryResult(Instructor Instructor, Course[] Courses);

internal class GetInstructorEditFormQueryHandler(
    IInstructorsRoRepository instructorsRepository,
    ICoursesRoRepository coursesRepository) :
    IRequestHandler<GetInstructorEditFormQuery, GetInstructorEditFormQueryResult>
{
    private readonly ICoursesRoRepository _coursesRepository = coursesRepository;
    private readonly IInstructorsRoRepository _instructorsRepository = instructorsRepository;

    public async Task<GetInstructorEditFormQueryResult> Handle(
        GetInstructorEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Instructor instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);
        if (instructor == null)
        {
            throw new EntityNotFoundException(nameof(instructor), request.Id);
        }

        Course[] courses = await _coursesRepository.GetAll(cancellationToken);

        return new GetInstructorEditFormQueryResult(instructor, courses);
    }
}
