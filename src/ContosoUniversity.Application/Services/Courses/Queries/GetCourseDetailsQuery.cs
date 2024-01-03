namespace ContosoUniversity.Application.Services.Courses.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetCourseDetailsQuery(Guid Id) : IRequest<GetCourseDetailsQueryResult>;

public record GetCourseDetailsQueryResult(Course Course, Department Department);

public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, GetCourseDetailsQueryResult>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public GetCourseDetailsQueryHandler(
        ICoursesRoRepository coursesRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _coursesRepository = coursesRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<GetCourseDetailsQueryResult> Handle(
        GetCourseDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Course course = await _coursesRepository.GetById(request.Id, cancellationToken);
        if (course == null)
            throw new EntityNotFoundException(nameof(course), request.Id);

        Department department = await _departmentsRepository.GetById(course.DepartmentId, cancellationToken);
        if (department == null)
            throw new EntityNotFoundException(nameof(department), course.DepartmentId);

        return new GetCourseDetailsQueryResult(course, department);
    }
}
