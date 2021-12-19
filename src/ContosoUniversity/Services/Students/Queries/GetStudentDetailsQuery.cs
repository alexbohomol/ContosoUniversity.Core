namespace ContosoUniversity.Services.Students.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

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

#warning This can be converted into reference for course titles
        Course[] courses = await _coursesRepository.GetByIds(coursesIds, cancellationToken);
        Dictionary<Guid, string> courseTitles = courses.ToDictionary(x => x.ExternalId, x => x.Title);

        return new GetStudentDetailsQueryResult(student, courseTitles);
    }
}