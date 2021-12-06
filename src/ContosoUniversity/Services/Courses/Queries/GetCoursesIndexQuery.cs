namespace ContosoUniversity.Services.Courses.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Course;

using MediatR;

using ViewModels.Courses;

public record GetCoursesIndexQuery : IRequest<List<CourseListItemViewModel>>;

public class GetCoursesIndexQueryHandler : IRequestHandler<GetCoursesIndexQuery, List<CourseListItemViewModel>>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRepository _departmentsRepository;

    public GetCoursesIndexQueryHandler(
        ICoursesRoRepository coursesRepository,
        IDepartmentsRepository departmentsRepository)
    {
        _coursesRepository = coursesRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<List<CourseListItemViewModel>> Handle(GetCoursesIndexQuery request,
        CancellationToken cancellationToken)
    {
        CourseReadModel[] courses = await _coursesRepository.GetAll(cancellationToken);

        Dictionary<Guid, string> departmentNames =
            await _departmentsRepository.GetDepartmentNamesReference(cancellationToken);

        CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

        return courses.Select(x => new CourseListItemViewModel
        {
            CourseCode = x.Code,
            Title = x.Title,
            Credits = x.Credits,
            Department = departmentNames[x.DepartmentId],
            Id = x.ExternalId
        }).ToList();
    }
}