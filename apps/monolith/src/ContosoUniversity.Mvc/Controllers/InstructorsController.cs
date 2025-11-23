namespace ContosoUniversity.Mvc.Controllers;

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

using Filters;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ViewModels;
using ViewModels.Instructors;

public class InstructorsController(IMediator mediator, ILogger<InstructorsController> logger) : Controller
{
    public async Task<IActionResult> Index(
        Guid? id,
        Guid? courseExternalId,
        [FromServices] IInstructorsRoRepository instructorsRepository,
        [FromServices] IDepartmentsRoRepository departmentsRepository,
        [FromServices] ICoursesRoRepository coursesRepository,
        [FromServices] IStudentsRoRepository studentsRepository,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("User visited page {Action} at {Controller}", nameof(Index), nameof(InstructorsController));

        Instructor[] instructors = (await instructorsRepository.GetAll(cancellationToken))
            .OrderBy(x => x.LastName)
            .ToArray();

        Course[] courses = await coursesRepository.GetAll(cancellationToken);

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
                await departmentsRepository.GetDepartmentNamesReference(cancellationToken);

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
            Student[] students = await studentsRepository.GetStudentsEnrolledForCourses(
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
        if (id is null)
        {
            return NotFound();
        }

        Instructor instructor = await mediator.Send(
            new GetInstructorDetailsQuery(id.Value),
            cancellationToken);

        return instructor is not null
            ? View(new InstructorDetailsViewModel(instructor))
            : NotFound();
    }

    public async Task<IActionResult> Create(
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    {
        Course[] courses = await repository.GetAll(cancellationToken);

        return View(new CreateInstructorForm(courses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ServiceFilter<FillModelState<CreateInstructorRequest>>]
    public async Task<IActionResult> Create(
        CreateInstructorRequest request,
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            Course[] courses = await repository.GetAll(cancellationToken);

            return View(new CreateInstructorForm(courses)
            {
                Request = request
            });
        }

        await mediator.Send(
            new CreateInstructorCommand(
                request.LastName,
                request.FirstName,
                request.HireDate,
                request.SelectedCourses,
                request.Location),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return BadRequest();
        }

        (Instructor instructor, Course[] courses) = await mediator.Send(
            new GetInstructorEditFormQuery(id.Value),
            cancellationToken);

        return instructor is null
            ? NotFound()
            : View(new EditInstructorForm(instructor, courses)
            {
                Request = new EditInstructorRequest(instructor)
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ServiceFilter<FillModelState<EditInstructorRequest>>]
    public async Task<IActionResult> Edit(
        EditInstructorRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            (Instructor instructor, Course[] courses) = await mediator.Send(
                new GetInstructorEditFormQuery(request.ExternalId),
                cancellationToken);

            return View(new EditInstructorForm(instructor, courses)
            {
                Request = request
            });
        }

        await mediator.Send(
            new EditInstructorCommand(
                request.ExternalId,
                request.LastName,
                request.FirstName,
                request.HireDate,
                request.SelectedCourses,
                request.Location),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return NotFound();
        }

        Instructor instructor = await mediator.Send(
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
        await mediator.Send(
            new DeleteInstructorCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
