namespace ContosoUniversity.Application.Courses.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetCourseDetailsQuery(Guid Id) : IRequest<GetCourseDetailsQueryResult>;

public record GetCourseDetailsQueryResult(Course Course, Department Department);

internal class GetCourseDetailsQueryHandler(
    ICoursesApiClient coursesApiClient,
    IDepartmentsApiClient departmentsApiClient)
    : IRequestHandler<GetCourseDetailsQuery, GetCourseDetailsQueryResult>
{
    public async Task<GetCourseDetailsQueryResult> Handle(
        GetCourseDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course course = await coursesApiClient.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new EntityNotFoundException(nameof(course), request.Id);
        }

        Department department = await departmentsApiClient.GetById(course.DepartmentId, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department), course.DepartmentId);
        }

        return new GetCourseDetailsQueryResult(course, department);
    }
}
