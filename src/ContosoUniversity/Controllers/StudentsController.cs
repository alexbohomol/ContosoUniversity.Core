namespace ContosoUniversity.Controllers;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Services.Students.Commands;
using Services.Students.Queries;

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
        return View(
            await _mediator.Send(
                request,
                cancellationToken));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        StudentDetailsViewModel result = await _mediator.Send(
            new GetStudentDetailsQuery(id.Value),
            cancellationToken);

        return result is not null
            ? View(result)
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

        EditStudentForm result = await _mediator.Send(
            new GetStudentEditFormQuery(id.Value),
            cancellationToken);

        return result is not null
            ? View(result)
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

        StudentDeletePageViewModel result = await _mediator.Send(
            new GetStudentDeletePageQuery(id.Value),
            cancellationToken);

        return result is not null
            ? View(result)
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