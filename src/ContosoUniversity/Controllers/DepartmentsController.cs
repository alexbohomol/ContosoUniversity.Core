namespace ContosoUniversity.Controllers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Services.Departments.Commands;
using Services.Departments.Queries;

using ViewModels;
using ViewModels.Departments;

public class DepartmentsController : Controller
{
    private readonly IInstructorsRoRepository _instructorsRepository;
    private readonly IMediator _mediator;

    public DepartmentsController(
        IInstructorsRoRepository instructorsRepository,
        IMediator mediator)
    {
        _instructorsRepository = instructorsRepository;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(
            await _mediator.Send(
                new GetDepartmentsIndexQuery(),
                cancellationToken));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        DepartmentDetailsViewModel result = await _mediator.Send(
            new GetDepartmentDetailsQuery(id.Value),
            cancellationToken);

        return result is not null
            ? View(result)
            : NotFound();
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        Dictionary<Guid, string> instructorNames =
            await _instructorsRepository.GetInstructorNamesReference(cancellationToken);

        return View(new CreateDepartmentForm
        {
            StartDate = DateTime.Now,
            InstructorsDropDown = instructorNames.ToSelectList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return BadRequest();

        if (!ModelState.IsValid)
            return View(
                new CreateDepartmentForm(
                    command,
                    await _instructorsRepository.GetInstructorNamesReference(cancellationToken)));

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null) return BadRequest();

        EditDepartmentForm result = await _mediator.Send(
            new GetDepartmentEditFormQuery(id.Value),
            cancellationToken);

        return result is not null
            ? View(result)
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditDepartmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return BadRequest();

        if (!ModelState.IsValid)
            return View(
                new EditDepartmentForm(
                    command,
                    await _instructorsRepository.GetInstructorNamesReference(cancellationToken)));

        await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null) return BadRequest();

        DepartmentDetailsViewModel result = await _mediator.Send(
            new GetDepartmentDetailsQuery(id.Value),
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
            new DeleteDepartmentCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}