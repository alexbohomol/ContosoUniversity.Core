namespace ContosoUniversity.Application.Services.Courses.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using global::Courses.Core;
using global::Courses.Core.Projections;

using MediatR;

public record GetCourseDetailsQuery(Guid Id) : IRequest<GetCourseDetailsQueryResult>;

public record GetCourseDetailsQueryResult(Course Course, Department Department);

internal class GetCourseDetailsQueryHandler(
    ICoursesRoRepository coursesRepository,
    IDepartmentsRoRepository departmentsRepository)
    : IRequestHandler<GetCourseDetailsQuery, GetCourseDetailsQueryResult>
{
    public async Task<GetCourseDetailsQueryResult> Handle(
        GetCourseDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course course = await coursesRepository.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new EntityNotFoundException(nameof(course), request.Id);
        }

        Department department = await departmentsRepository.GetById(course.DepartmentId, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department), course.DepartmentId);
        }

        return new GetCourseDetailsQueryResult(course, department);
    }
}
