namespace ContosoUniversity.Application.Services.Students.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Courses.Core;

using global::Students.Core;
using global::Students.Core.Projections;

using MediatR;

using SharedKernel.Exceptions;

public record GetStudentDetailsQuery(Guid Id) : IRequest<GetStudentDetailsQueryResult>;

public record GetStudentDetailsQueryResult(
    Student Student,
    Dictionary<Guid, string> CourseTitles);

internal class GetStudentDetailsQueryHandler(
    IStudentsRoRepository studentsRepository,
    ICoursesRoRepository coursesRepository)
    : IRequestHandler<GetStudentDetailsQuery, GetStudentDetailsQueryResult>
{
    public async Task<GetStudentDetailsQueryResult> Handle(
        GetStudentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Student student = await studentsRepository.GetById(request.Id, cancellationToken);
        if (student is null)
        {
            throw new EntityNotFoundException(nameof(student), request.Id);
        }

        Guid[] coursesIds = student.Enrollments.Select(x => x.CourseId).ToArray();

        Dictionary<Guid, string> courseTitles = await coursesRepository
            .GetCourseTitlesReference(coursesIds, cancellationToken);

        Guid[] notFoundCourseIds = coursesIds.Except(courseTitles.Keys).ToArray();
        if (notFoundCourseIds.Any())
        {
            throw new AggregateException(notFoundCourseIds
                .Select(x => new EntityNotFoundException("course", x)));
        }

        return new GetStudentDetailsQueryResult(student, courseTitles);
    }
}
