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
using Application.Services.Instructors.Validators;

using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels;
using ViewModels.Instructors;

public class InstructorsController(
    IInstructorsRoRepository instructorsRepository,
    IDepartmentsRoRepository departmentsRepository,
    ICoursesRoRepository coursesRepository,
    IStudentsRoRepository studentsRepository,
    IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(Guid? id, Guid? courseExternalId, CancellationToken cancellationToken)
    {
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

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        Course[] courses = await coursesRepository.GetAll(cancellationToken);

        return View(new CreateInstructorForm
        {
            HireDate = DateTime.Now,
            AssignedCourses = courses.ToAssignedCourseOptions()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateInstructorRequest request,
        [FromServices] CreateInstructorCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Course[] courses = await coursesRepository.GetAll(cancellationToken);

            return View(
                new CreateInstructorForm(
                    request,
                    courses.ToAssignedCourseOptions()));
        }

        CreateInstructorCommand command = new()
        {
            LastName = request.LastName,
            FirstName = request.FirstName,
            HireDate = request.HireDate,
            SelectedCourses = request.SelectedCourses,
            Location = request.Location
        };
        ValidationResult result = validator.Validate(command);
        if (!result.IsValid)
        {
            return BadRequest();
        }

        await mediator.Send(command, cancellationToken);

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
            : View(new EditInstructorForm(instructor, courses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditInstructorRequest request,
        [FromServices] EditInstructorCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Course[] courses = await coursesRepository.GetAll(cancellationToken);

            return View(
                new EditInstructorForm(
                    request,
                    courses.ToAssignedCourseOptions( /* instructor? */)));
        }

        EditInstructorCommand command = new()
        {
            ExternalId = request.ExternalId,
            LastName = request.LastName,
            FirstName = request.FirstName,
            HireDate = request.HireDate,
            SelectedCourses = request.SelectedCourses,
            Location = request.Location
        };
        ValidationResult result = validator.Validate(command);
        if (!result.IsValid)
        {
            return BadRequest();
        }

        await mediator.Send(command, cancellationToken);

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
