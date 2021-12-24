namespace ContosoUniversity.Application.Services.Students.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetStudentDetailsQuery(Guid Id) : IRequest<GetStudentDetailsQueryResult>;

public record GetStudentDetailsQueryResult(
    Student Student,
    Dictionary<Guid, string> CourseTitles);

public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, GetStudentDetailsQueryResult>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IStudentsRoRepository _studentsRepository;

    public GetStudentDetailsQueryHandler(
        IStudentsRoRepository studentsRepository,
        ICoursesRoRepository coursesRepository)
    {
        _studentsRepository = studentsRepository;
        _coursesRepository = coursesRepository;
    }

    public async Task<GetStudentDetailsQueryResult> Handle(
        GetStudentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Student student = await _studentsRepository.GetById(request.Id, cancellationToken);
        if (student == null)
            throw new EntityNotFoundException(nameof(student), request.Id);

        Guid[] coursesIds = student.Enrollments.Select(x => x.CourseId).ToArray();

        Dictionary<Guid, string> courseTitles = await _coursesRepository
            .GetCourseNamesReference(coursesIds, cancellationToken);

        Guid[] notFoundCourseIds = coursesIds.Except(courseTitles.Keys).ToArray();
        if (notFoundCourseIds.Any())
            throw new AggregateException(notFoundCourseIds
                .Select(x => new EntityNotFoundException("course", x)));

        return new GetStudentDetailsQueryResult(student, courseTitles);
    }
}