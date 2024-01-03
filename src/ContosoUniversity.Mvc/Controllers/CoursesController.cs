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

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Courses;

public class CoursesController : Controller
{
    private readonly ICoursesRoRepository _coursesRoRepository;
    private readonly ICoursesRwRepository _coursesRwRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IMediator _mediator;

    public CoursesController(
        IDepartmentsRoRepository departmentsRepository,
        ICoursesRoRepository coursesRoRepository,
        ICoursesRwRepository coursesRwRepository,
        IMediator mediator)
    {
        _departmentsRepository = departmentsRepository;
        _coursesRoRepository = coursesRoRepository;
        _coursesRwRepository = coursesRwRepository;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        (Course[] courses, Dictionary<Guid, string> departmentsReference) = await _mediator.Send(
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
            return BadRequest();

        (Course course, Department department) = await _mediator.Send(
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
                await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(
                new CreateCourseForm(
                    command,
                    await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
            return BadRequest();

        (Course course, Dictionary<Guid, string> departmentsReference) = await _mediator.Send(
            new GetCourseEditFormQuery(id.Value),
            cancellationToken);

        return course is not null
            ? View(new CourseEditForm(
                new EditCourseCommand(course),
                course.Code,
                departmentsReference))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCourseCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            Course course = await _coursesRoRepository.GetById(command.Id, cancellationToken);

            return View(
                new CourseEditForm(
                    command,
                    course.Code,
                    await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
        }

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
            return BadRequest();

        (Course course, Department department) = await _mediator.Send(
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
        await _mediator.Send(
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
            ViewData["RowsAffected"] = await _coursesRwRepository.UpdateCourseCredits(
                multiplier.Value,
                cancellationToken);

        return View();
    }
}
