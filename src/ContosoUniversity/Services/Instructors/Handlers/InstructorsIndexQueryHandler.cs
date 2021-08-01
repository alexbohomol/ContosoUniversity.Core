namespace ContosoUniversity.Services.Instructors.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Student;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries;

    using ViewModels;
    using ViewModels.Instructors;

    public class InstructorsIndexQueryHandler : IRequestHandler<InstructorsIndexQuery, InstructorIndexViewModel>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IStudentsRepository _studentsRepository;

        public InstructorsIndexQueryHandler(
            DepartmentsContext departmentsContext,
            IDepartmentsRepository departmentsRepository,
            ICoursesRepository coursesRepository,
            IStudentsRepository studentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
            _coursesRepository = coursesRepository;
            _studentsRepository = studentsRepository;
        }
        
        public async Task<InstructorIndexViewModel> Handle(InstructorsIndexQuery request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var courseExternalId = request.CourseExternalId;
            
            var instructors = await _departmentsContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .OrderBy(i => i.LastName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var courses = await _coursesRepository.GetAll();

            CrossContextBoundariesValidator.EnsureInstructorsReferenceTheExistingCourses(instructors, courses);

            var viewModel = new InstructorIndexViewModel
            {
                Instructors = instructors.Select(x =>
                {
                    var assignedCourseIds = x.CourseAssignments.Select(ca => ca.CourseExternalId).ToArray();

                    return new InstructorListItemViewModel
                    {
                        Id = x.ExternalId,
                        FirstName = x.FirstMidName,
                        LastName = x.LastName,
                        HireDate = x.HireDate,
                        Office = x.OfficeAssignment?.Location,
                        AssignedCourseIds = assignedCourseIds,
                        AssignedCourses = courses
                            .Where(c => assignedCourseIds.Contains(c.EntityId))
                            .Select(c => $"{c.Code} {c.Title}"),
                        RowClass = id is not null && id == x.ExternalId
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