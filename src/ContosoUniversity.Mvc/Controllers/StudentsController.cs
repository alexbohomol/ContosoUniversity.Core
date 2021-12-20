namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly.Paging;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Students.Commands;
using Application.Services.Students.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Students;

public class StudentsController : Controller
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetStudentsIndexQuery request, CancellationToken cancellationToken)
    {
        if (request.SearchString != null)
            request.PageNumber = 1;
        else
            request.SearchString = request.CurrentFilter;

        (PageInfo pageInfo, Student[] students) = await _mediator.Send(request, cancellationToken);

        return View(new StudentIndexViewModel(request, pageInfo, students));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        (Student student, Dictionary<Guid, string> courseTitles) = await _mediator.Send(
            new GetStudentDetailsQuery(id.Value),
            cancellationToken);

        return student is not null
            ? View(new StudentDetailsViewModel(student, courseTitles))
            : NotFound();
    }

    public IActionResult Create()
    {
        return View(new CreateStudentForm
        {
            EnrollmentDate = DateTime.Now
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStudentCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(command as CreateStudentForm);

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null) return BadRequest();

        Student student = await _mediator.Send(
            new GetStudentProjectionQuery(id.Value),
            cancellationToken);

        return student is not null
            ? View(new EditStudentForm(student))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditStudentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return BadRequest();

        if (!ModelState.IsValid) return View(command as EditStudentForm);

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null) return BadRequest();

        Student student = await _mediator.Send(
            new GetStudentProjectionQuery(id.Value),
            cancellationToken);

        return student is not null
            ? View(new StudentDeletePageViewModel(student))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteStudentCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}