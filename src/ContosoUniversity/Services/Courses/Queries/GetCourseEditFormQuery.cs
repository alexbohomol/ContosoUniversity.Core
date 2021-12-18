namespace ContosoUniversity.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

using Commands;

using MediatR;

using ViewModels.Courses;

public record GetCourseEditFormQuery(Guid Id) : IRequest<CourseEditForm>;

public class GetCourseEditFormQueryHandler : IRequestHandler<GetCourseEditFormQuery, CourseEditForm>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public GetCourseEditFormQueryHandler(
        ICoursesRoRepository coursesRepository,
        IDepartmentsRoRepository departmentsRepository)
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