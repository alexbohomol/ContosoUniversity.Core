namespace ContosoUniversity.Application.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetCourseEditFormQuery(Guid Id) : IRequest<GetCourseEditFormQueryResult>;

public record GetCourseEditFormQueryResult(Course Course, Dictionary<Guid, string> DepartmentsReference);

internal class GetCourseEditFormQueryHandler(
    ICoursesRoRepository coursesRepository,
    IDepartmentsRoRepository departmentsRepository) : IRequestHandler<GetCourseEditFormQuery, GetCourseEditFormQueryResult>
{
    public async Task<GetCourseEditFormQueryResult> Handle(
        GetCourseEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course course = await coursesRepository.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new EntityNotFoundException(nameof(course), request.Id);
        }

        Dictionary<Guid, string> departments =
            await departmentsRepository.GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(
            new[] { course },
            departments.Keys);

        return new GetCourseEditFormQueryResult(course, departments);
    }
}
