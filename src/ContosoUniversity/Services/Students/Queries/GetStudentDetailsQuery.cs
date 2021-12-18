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

using ViewModels;
using ViewModels.Students;

public record GetStudentDetailsQuery(Guid Id) : IRequest<StudentDetailsViewModel>;

public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetailsViewModel>
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

    public async Task<StudentDetailsViewModel> Handle(GetStudentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        Student student = await _studentsRepository.GetById(request.Id, cancellationToken);
        if (student == null)
            throw new EntityNotFoundException(nameof(student), request.Id);

        Guid[] coursesIds = student.Enrollments.Select(x => x.CourseId).ToArray();

        Dictionary<Guid, string> courseTitles = (await _coursesRepository.GetByIds(coursesIds, cancellationToken))
            .ToDictionary(x => x.ExternalId, x => x.Title);

        return new StudentDetailsViewModel
        {
            LastName = student.LastName,
            FirstMidName = student.FirstName,
            EnrollmentDate = student.EnrollmentDate,
            ExternalId = student.ExternalId,
            Enrollments = student.Enrollments.Select(x => new EnrollmentViewModel
            {
                CourseTitle = courseTitles[x.CourseId],
                Grade = x.Grade.ToDisplayString()
            }).ToArray()
        };
    }
}