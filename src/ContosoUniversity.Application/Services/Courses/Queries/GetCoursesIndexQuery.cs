namespace ContosoUniversity.Application.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using MediatR;

public record GetCoursesIndexQuery : IRequest<GetCoursesIndexQueryResult>;

public record GetCoursesIndexQueryResult(
    Course[] Courses,
    Dictionary<Guid, string> DepartmentsReference);

internal class GetCoursesIndexQueryHandler(
    ICoursesRoRepository coursesRepository,
    IDepartmentsRoRepository departmentsRepository) : IRequestHandler<GetCoursesIndexQuery, GetCoursesIndexQueryResult>
{
    private readonly ICoursesRoRepository _coursesRepository = coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository = departmentsRepository;

    public async Task<GetCoursesIndexQueryResult> Handle(
        GetCoursesIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course[] courses = await _coursesRepository.GetAll(cancellationToken);

        Dictionary<Guid, string> departmentNames = await _departmentsRepository
            .GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

        return new GetCoursesIndexQueryResult(courses, departmentNames);
    }
}
