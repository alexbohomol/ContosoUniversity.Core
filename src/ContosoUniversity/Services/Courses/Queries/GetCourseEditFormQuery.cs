namespace ContosoUniversity.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Course;

using MediatR;

using ViewModels.Courses;

public record GetCourseEditFormQuery(Guid Id) : IRequest<CourseEditForm>;

public class GetCourseEditFormQueryHandler : IRequestHandler<GetCourseEditFormQuery, CourseEditForm>
{
    private readonly ICoursesRwRepository _coursesRepository;
    private readonly IDepartmentsRepository _departmentsRepository;

    public GetCourseEditFormQueryHandler(
        ICoursesRwRepository coursesRepository,
        IDepartmentsRepository departmentsRepository)
    {
        _coursesRepository = coursesRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<CourseEditForm> Handle(GetCourseEditFormQuery request, CancellationToken cancellationToken)
    {
        Course course = await _coursesRepository.GetById(request.Id, cancellationToken);
        if (course == null)
            throw new EntityNotFoundException(nameof(course), request.Id);

        Dictionary<Guid, string> departments =
            await _departmentsRepository.GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(
            new[] { course },
            departments.Keys);

        return new CourseEditForm(
            new EditCourseCommand(course),
            course.Code,
            departments);
    }
}