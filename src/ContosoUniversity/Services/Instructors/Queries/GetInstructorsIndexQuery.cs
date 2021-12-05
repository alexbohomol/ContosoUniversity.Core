namespace ContosoUniversity.Services.Instructors.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Course;
using Domain.Instructor;
using Domain.Student;

using MediatR;

using ViewModels;
using ViewModels.Instructors;

public record GetInstructorsIndexQuery(Guid? Id, Guid? CourseExternalId) : IRequest<InstructorIndexViewModel>;

public class GetInstructorsIndexQueryHandler : IRequestHandler<GetInstructorsIndexQuery, InstructorIndexViewModel>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IInstructorsRepository _instructorsRepository;
    private readonly IStudentsRepository _studentsRepository;

    public GetInstructorsIndexQueryHandler(
        IInstructorsRepository instructorsRepository,
        IDepartmentsRepository departmentsRepository,
        ICoursesRoRepository coursesRepository,
        IStudentsRepository studentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
        _coursesRepository = coursesRepository;
        _studentsRepository = studentsRepository;
    }

    public async Task<InstructorIndexViewModel> Handle(GetInstructorsIndexQuery request,
        CancellationToken cancellationToken)
    {
        Guid? id = request.Id;
        Guid? courseExternalId = request.CourseExternalId;

        Instructor[] instructors = (await _instructorsRepository.GetAll(cancellationToken))
            .OrderBy(x => x.LastName)
            .ToArray();

        Course[] courses = await _coursesRepository.GetAll(cancellationToken);

        CrossContextBoundariesValidator.EnsureInstructorsReferenceTheExistingCourses(instructors, courses);

        var viewModel = new InstructorIndexViewModel
        {
            Instructors = instructors.Select(x =>
            {
                return new InstructorListItemViewModel
                {
                    Id = x.ExternalId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate,
                    Office = x.Office?.Title,
                    AssignedCourseIds = x.Courses,
                    AssignedCourses = courses
                        .Where(c => x.Courses.Contains(c.ExternalId))
                        .Select(c => $"{c.Code} {c.Title}"),
                    RowClass = id is not null && id == x.ExternalId
                        ? "table-success"
                        : string.Empty
                };
            }).ToArray()
        };

        if (id is not null)
        {
            InstructorListItemViewModel instructor = viewModel.Instructors.Single(i => i.Id == id.Value);
            HashSet<Guid> instructorCourseIds = instructor.AssignedCourseIds.ToHashSet();
            Dictionary<Guid, string> departmentNames =
                await _departmentsRepository.GetDepartmentNamesReference(cancellationToken);

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

            viewModel.Courses = courses
                .Where(x => instructorCourseIds.Contains(x.ExternalId))
                .Select(x => new CourseListItemViewModel
                {
                    Id = x.ExternalId,
                    CourseCode = x.Code,
                    Title = x.Title,
                    Department = departmentNames[x.DepartmentId],
                    RowClass = courseExternalId is not null && courseExternalId == x.ExternalId
                        ? "table-success"
                        : string.Empty
                }).ToList();
        }

        if (courseExternalId is not null)
        {
            Student[] students = await _studentsRepository.GetStudentsEnrolledForCourses(
                new[]
                {
                    courseExternalId.Value
                },
                cancellationToken);

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