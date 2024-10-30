namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Courses.Commands;
using Application.Services.Courses.Queries;

using Courses.Core;
using Courses.Core.Projections;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Courses;

public class CoursesController(IMediator mediator) : Controller
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

    public async Task<IActionResult> Create(
        [FromServices] IDepartmentsRoRepository repository,
        CancellationToken cancellationToken)
    {
        var departmentNames = await repository.GetDepartmentNamesReference(cancellationToken);
        return View(new CreateCourseForm(departmentNames));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCourseRequest request,
        [FromServices] IDepartmentsRoRepository repository,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var departmentNames = await repository.GetDepartmentNamesReference(cancellationToken);
            return View(new CreateCourseForm(departmentNames)
            {
                Request = request
            });
        }

        await mediator.Send(
            new CreateCourseCommand(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId),
            cancellationToken);

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
            : View(new EditCourseForm(course.Code, departmentsReference)
            {
                Request = new EditCourseRequest(course)
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditCourseRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            (Course course, Dictionary<Guid, string> departmentsReference) = await mediator.Send(
                new GetCourseEditFormQuery(request.Id),
                cancellationToken);

            return View(new EditCourseForm(course.Code, departmentsReference)
            {
                Request = request
            });
        }

        await mediator.Send(
            new EditCourseCommand(
                request.Id,
                request.Title,
                request.Credits,
                request.DepartmentId),
            cancellationToken);

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
    public async Task<IActionResult> UpdateCourseCredits(
        int? multiplier,
        [FromServices] ICoursesRwRepository coursesRepository,
        CancellationToken cancellationToken)
    {
        if (multiplier.HasValue)
        {
            ViewData["RowsAffected"] = await coursesRepository.UpdateCourseCredits(
                multiplier.Value,
                cancellationToken);
        }

        return View();
    }
}
