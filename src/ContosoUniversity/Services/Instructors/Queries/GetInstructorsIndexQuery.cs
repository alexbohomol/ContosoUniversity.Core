namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Student;

    using MediatR;

    using ViewModels;
    using ViewModels.Instructors;

    public record GetInstructorsIndexQuery(Guid? Id, Guid? CourseExternalId) : IRequest<InstructorIndexViewModel>;
    
    public class GetInstructorsIndexQueryHandler : IRequestHandler<GetInstructorsIndexQuery, InstructorIndexViewModel>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IStudentsRepository _studentsRepository;

        public GetInstructorsIndexQueryHandler(
            IInstructorsRepository instructorsRepository,
            IDepartmentsRepository departmentsRepository,
            ICoursesRepository coursesRepository,
            IStudentsRepository studentsRepository)
        {
            _instructorsRepository = instructorsRepository;
            _departmentsRepository = departmentsRepository;
            _coursesRepository = coursesRepository;
            _studentsRepository = studentsRepository;
        }
        
        public async Task<InstructorIndexViewModel> Handle(GetInstructorsIndexQuery request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var courseExternalId = request.CourseExternalId;
            
            var instructors = (await _instructorsRepository.GetAll())
                .OrderBy(x => x.LastName)
                .ToArray();

            var courses = await _coursesRepository.GetAll();

            CrossContextBoundariesValidator.EnsureInstructorsReferenceTheExistingCourses(instructors, courses);

            var viewModel = new InstructorIndexViewModel
            {
                Instructors = instructors.Select(x =>
                {
                    var assignedCourseIds = x.Courses.Select(ca => ca.CourseId).ToArray();

                    return new InstructorListItemViewModel
                    {
                        Id = x.EntityId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        HireDate = x.HireDate,
                        Office = x.Office?.Title,
                        AssignedCourseIds = assignedCourseIds,
                        AssignedCourses = courses
                            .Where(c => assignedCourseIds.Contains(c.EntityId))
                            .Select(c => $"{c.Code} {c.Title}"),
                        RowClass = id is not null && id == x.EntityId
                            ? "table-success"
                            : string.Empty
                    };
                }).ToArray()
            };

            if (id is not null)
            {
                var instructor = viewModel.Instructors.Single(i => i.Id == id.Value);
                var instructorCourseIds = instructor.AssignedCourseIds.ToHashSet();
                var departmentNames = await _departmentsRepository.GetDepartmentNamesReference();
                
                CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);
                
                viewModel.Courses = courses
                    .Where(x => instructorCourseIds.Contains(x.EntityId))
                    .Select(x => new CourseListItemViewModel
                    {
                        Id = x.EntityId,
                        CourseCode = x.Code,
                        Title = x.Title,
                        Department = departmentNames[x.DepartmentId],
                        RowClass = courseExternalId is not null && courseExternalId == x.EntityId
                            ? "table-success"
                            : string.Empty
                    }).ToList();
            }

            if (courseExternalId is not null)
            {
                var students = await _studentsRepository.GetStudentsEnrolledForCourses(new[]
                {
                    courseExternalId.Value
                });
                
                CrossContextBoundariesValidator.EnsureEnrollmentsReferenceTheExistingCourses(
                    students.SelectMany(x => x.Enrollments).Distinct(), 
                    courses);
                
                viewModel.Students = students.Select(x => new EnrolledStudentViewModel
                {
                    StudentFullName = x.FullName(),
                    EnrollmentGrade = x.Enrollments[courseExternalId.Value].Grade.ToDisplayString()
                });
            }

            return viewModel;
        }
    }
}