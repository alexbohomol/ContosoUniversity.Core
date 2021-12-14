namespace ContosoUniversity.Services.Courses.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Course;
using Domain.Department;

using MediatR;

using ViewModels.Courses;

public record GetCourseDetailsQuery(Guid Id) : IRequest<CourseDetailsViewModel>;

public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetailsViewModel>
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

    public async Task<CourseDetailsViewModel> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        CourseReadModel course = await _coursesRepository.GetById(request.Id, cancellationToken);
        if (course == null)
            throw new EntityNotFoundException(nameof(course), request.Id);

        DepartmentReadModel department = await _departmentsRepository.GetById(course.DepartmentId, cancellationToken);
        if (department == null)
            throw new EntityNotFoundException(nameof(department), course.DepartmentId);

        return new CourseDetailsViewModel
        {
            CourseCode = course.Code,
            Title = course.Title,
            Credits = course.Credits,
            Department = department.Name,
            Id = course.ExternalId
        };
    }
}