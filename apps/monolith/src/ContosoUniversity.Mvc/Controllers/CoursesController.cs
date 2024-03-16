namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Contracts.Repositories.ReadWrite;
using Application.Services.Courses.Commands;
using Application.Services.Courses.Queries;
using Application.Services.Courses.Validators;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Courses;

public class CoursesController(
    IDepartmentsRoRepository departmentsRepository,
    ICoursesRoRepository coursesRoRepository,
    ICoursesRwRepository coursesRwRepository,
    IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        (Course[] courses, Dictionary<Guid, string> departmentsReference) = await mediator.Send(
            new GetCoursesIndexQuery(),
            cancellationToken);

        return View(courses.Select(x => new CourseListItemViewModel
        {
            CourseCode = x.Code,
            Title = x.Title,
            Credits = x.Credits,
            Department = departmentsReference[x.DepartmentId],
            Id = x.ExternalId
        }));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return BadRequest();
        }

        (Course course, Department department) = await mediator.Send(
            new GetCourseDetailsQuery(id.Value),
            cancellationToken);

        return course is not null
            ? View(new CourseDetailsViewModel(course, department))
            : NotFound();
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return View(
            new CreateCourseForm(
                await departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCourseRequest request,
        [FromServices] CreateCourseCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(
                new CreateCourseForm(
                    request,
                    await departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
        }

        CreateCourseCommand command = new()
        {
            CourseCode = request.CourseCode,
            Credits = request.Credits,
            DepartmentId = request.DepartmentId,
            Title = request.Title
        };
        var result = await validator.ValidateAsync(command, cancellationToken);
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

        (Course course, Dictionary<Guid, string> departmentsReference) = await mediator.Send(
            new GetCourseEditFormQuery(id.Value),
            cancellationToken);

        return course is null
            ? NotFound()
            : View(new EditCourseForm(
                new EditCourseRequest(course),
                course.Code,
                departmentsReference));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditCourseRequest request,
        [FromServices] EditCourseCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Course course = await coursesRoRepository.GetById(request.Id, cancellationToken);

            return View(
                new EditCourseForm(
                    request,
                    course.Code,
                    await departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
        }

        EditCourseCommand command = new()
        {
            Id = request.Id,
            Credits = request.Credits,
            DepartmentId = request.DepartmentId,
            Title = request.Title
        };
        var result = await validator.ValidateAsync(command, cancellationToken);
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
            return BadRequest();
        }

        (Course course, Department department) = await mediator.Send(
            new GetCourseDetailsQuery(id.Value),
            cancellationToken);

        return course is not null
            ? View(new CourseDetailsViewModel(course, department))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new DeleteCourseCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult UpdateCourseCredits()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCourseCredits(int? multiplier, CancellationToken cancellationToken)
    {
        if (multiplier.HasValue)
        {
            ViewData["RowsAffected"] = await coursesRwRepository.UpdateCourseCredits(
                multiplier.Value,
                cancellationToken);
        }

        return View();
    }
}
