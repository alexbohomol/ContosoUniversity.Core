namespace ContosoUniversity.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services;
using Application.Services.Instructors.Commands;
using Application.Services.Instructors.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels;
using ViewModels.Instructors;

public class InstructorsController : Controller
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRepository;
    private readonly IMediator _mediator;
    private readonly IStudentsRoRepository _studentsRepository;

    public InstructorsController(
        IInstructorsRoRepository instructorsRepository,
        IDepartmentsRoRepository departmentsRepository,
        ICoursesRoRepository coursesRepository,
        IStudentsRoRepository studentsRepository,
        IMediator mediator)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
        _coursesRepository = coursesRepository;
        _studentsRepository = studentsRepository;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(Guid? id, Guid? courseExternalId, CancellationToken cancellationToken)
    {
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
                    Office = x.Office,
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
                StudentFullName = x.FullName,
                EnrollmentGrade = x.Enrollments
                    .Single(e => e.CourseId == courseExternalId.Value)
                    .Grade
                    .ToDisplayString()
            });
        }

        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        Instructor instructor = await _mediator.Send(
            new GetInstructorDetailsQuery(id.Value),
            cancellationToken);

        return instructor is not null
            ? View(new InstructorDetailsViewModel(instructor))
            : NotFound();
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        Course[] courses = await _coursesRepository.GetAll(cancellationToken);

        return View(new CreateInstructorForm
        {
            HireDate = DateTime.Now,
            AssignedCourses = courses.ToAssignedCourseOptions()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateInstructorCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            Course[] courses = await _coursesRepository.GetAll(cancellationToken);

            return View(
                new CreateInstructorForm(
                    command,
                    courses.ToAssignedCourseOptions()));
        }

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null) return BadRequest();

        (Instructor instructor, Course[] courses) = await _mediator.Send(
            new GetInstructorEditFormQuery(id.Value),
            cancellationToken);

        return instructor is not null
            ? View(new EditInstructorForm(instructor, courses))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditInstructorCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            Course[] courses = await _coursesRepository.GetAll(cancellationToken);

            return View(
                new EditInstructorForm(
                    command,
                    courses.ToAssignedCourseOptions( /* instructor? */)));
        }

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        Instructor instructor = await _mediator.Send(
            new GetInstructorDetailsQuery(id.Value),
            cancellationToken);

        return instructor is not null
            ? View(new InstructorDetailsViewModel(instructor))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteInstructorCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}