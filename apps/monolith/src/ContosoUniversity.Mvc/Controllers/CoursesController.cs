namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Contracts.Repositories.ReadOnly.Queries;
using Application.Contracts.Repositories.ReadWrite;
using Application.Services.Courses.Commands;
using Application.Services.Courses.Queries;

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

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        => await RenderDetailsPage(id, cancellationToken);

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
        => await RenderCreateForm(cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return await RenderCreateForm(cancellationToken, request);
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

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        => await RenderEditForm(id, cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditCourseRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return await RenderEditForm(request.Id, cancellationToken, request);
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

    public async Task<IActionResult> DeletePage(Guid id, CancellationToken cancellationToken)
        => await RenderDetailsPage(id, cancellationToken);

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

    #region Helpers

    private async Task<IActionResult> RenderCreateForm(
        CancellationToken cancellationToken,
        CreateCourseRequest request = null)
    {
        var departmentNames = await mediator.Send(
            new GetDepartmentNamesQuery(),
            cancellationToken);

        return View(new CreateCourseForm(departmentNames)
        {
            Request = request
        });
    }

    private async Task<IActionResult> RenderEditForm(
        Guid id,
        CancellationToken cancellationToken,
        EditCourseRequest request = null)
    {
        (Course course, Dictionary<Guid, string> departmentsReference) = await mediator.Send(
            new GetCourseEditFormQuery(id),
            cancellationToken);

        return course is null
            ? NotFound()
            : View(new EditCourseForm(course.Code, departmentsReference)
            {
                Request = request ?? new EditCourseRequest(course)
            });
    }

    private async Task<IActionResult> RenderDetailsPage(
        Guid id,
        CancellationToken cancellationToken)
    {
        (Course course, Department department) = await mediator.Send(
            new GetCourseDetailsQuery(id),
            cancellationToken);

        return course is not null
            ? View(new CourseDetailsViewModel(course, department))
            : NotFound();
    }

    #endregion
}
