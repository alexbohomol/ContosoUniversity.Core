using Course = Courses.Core.Projections.Course;
using Department = Departments.Core.Projections.Department;
using ICoursesRoRepository = Courses.Core.ICoursesRoRepository;
using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;

namespace ContosoUniversity.Application.Courses.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SharedKernel.Exceptions;

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
