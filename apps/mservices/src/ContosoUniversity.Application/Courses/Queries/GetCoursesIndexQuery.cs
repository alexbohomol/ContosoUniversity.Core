using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;

namespace ContosoUniversity.Application.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

public record GetCoursesIndexQuery : IRequest<GetCoursesIndexQueryResult>;

public record GetCoursesIndexQueryResult(
    Course[] Courses,
    Dictionary<Guid, string> DepartmentsReference);

internal class GetCoursesIndexQueryHandler(
    ICoursesApiClient coursesApiClient,
    IDepartmentsRoRepository departmentsRepository)
    : IRequestHandler<GetCoursesIndexQuery, GetCoursesIndexQueryResult>
{
    public async Task<GetCoursesIndexQueryResult> Handle(
        GetCoursesIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course[] courses = await coursesApiClient.GetAll(cancellationToken);

        Dictionary<Guid, string> departmentNames = await departmentsRepository
            .GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

        return new GetCoursesIndexQueryResult(courses, departmentNames);
    }
}
