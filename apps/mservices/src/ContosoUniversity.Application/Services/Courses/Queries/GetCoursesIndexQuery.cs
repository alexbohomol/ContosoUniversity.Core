namespace ContosoUniversity.Application.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using global::Courses.Core;
using global::Courses.Core.Projections;

using global::Departments.Core;

using MediatR;

public record GetCoursesIndexQuery : IRequest<GetCoursesIndexQueryResult>;

public record GetCoursesIndexQueryResult(
    Course[] Courses,
    Dictionary<Guid, string> DepartmentsReference);

internal class GetCoursesIndexQueryHandler(
    ICoursesRoRepository coursesRepository,
    IDepartmentsRoRepository departmentsRepository)
    : IRequestHandler<GetCoursesIndexQuery, GetCoursesIndexQueryResult>
{
    public async Task<GetCoursesIndexQueryResult> Handle(
        GetCoursesIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course[] courses = await coursesRepository.GetAll(cancellationToken);

        Dictionary<Guid, string> departmentNames = await departmentsRepository
            .GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

        return new GetCoursesIndexQueryResult(courses, departmentNames);
    }
}
