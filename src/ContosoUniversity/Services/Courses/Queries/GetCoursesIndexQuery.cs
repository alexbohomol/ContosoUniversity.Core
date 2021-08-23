namespace ContosoUniversity.Services.Courses.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using ViewModels.Courses;

    public record GetCoursesIndexQuery : IRequest<List<CourseListItemViewModel>>;
    
    public class GetCoursesIndexQueryHandler : IRequestHandler<GetCoursesIndexQuery, List<CourseListItemViewModel>>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public GetCoursesIndexQueryHandler(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
        }

        public async Task<List<CourseListItemViewModel>> Handle(GetCoursesIndexQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesRepository.GetAll(cancellationToken);

            var departmentNames = await _departmentsRepository.GetDepartmentNamesReference(cancellationToken);

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
}