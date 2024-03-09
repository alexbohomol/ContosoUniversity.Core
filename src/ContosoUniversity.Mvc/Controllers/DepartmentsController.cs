namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Departments.Commands;
using Application.Services.Departments.Queries;
using Application.Services.Departments.Validators;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels;
using ViewModels.Departments;

public class DepartmentsController(
    IInstructorsRoRepository instructorsRepository,
    IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        Department[] departments = await mediator.Send(
            new GetDepartmentsIndexQuery(),
            cancellationToken);

        return View(departments.Select(x => new DepartmentListItemViewModel(x)));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return BadRequest();
        }

        Department department = await mediator.Send(
            new GetDepartmentDetailsQuery(id.Value),
            cancellationToken);

        return department is not null
            ? View(new DepartmentDetailsViewModel(department))
            : NotFound();
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        Dictionary<Guid, string> instructorNames =
            await instructorsRepository.GetInstructorNamesReference(cancellationToken);

        return View(new CreateDepartmentForm
        {
            StartDate = DateTime.Now,
            InstructorsDropDown = instructorNames.ToSelectList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateDepartmentRequest request,
        [FromServices] CreateDepartmentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(
                new CreateDepartmentForm(
                    request,
                    await instructorsRepository.GetInstructorNamesReference(cancellationToken)));
        }

        CreateDepartmentCommand command = new()
        {
            AdministratorId = request.AdministratorId,
            Budget = request.Budget,
            Name = request.Name,
            StartDate = request.StartDate
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

        (Department department, Dictionary<Guid, string> instructorsReference) = await mediator.Send(
            new GetDepartmentEditFormQuery(id.Value),
            cancellationToken);

        return department is null
            ? NotFound()
            : View(new EditDepartmentForm(department, instructorsReference));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditDepartmentRequest request,
        [FromServices] EditDepartmentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(
                new EditDepartmentForm(
                    request,
                    await instructorsRepository.GetInstructorNamesReference(cancellationToken)));
        }

        EditDepartmentCommand command = new()
        {
            AdministratorId = request.AdministratorId,
            Budget = request.Budget,
            ExternalId = request.ExternalId,
            Name = request.Name,
            RowVersion = request.RowVersion,
            StartDate = request.StartDate
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

        Department department = await mediator.Send(
            new GetDepartmentDetailsQuery(id.Value),
            cancellationToken);

        return department is not null
            ? View(new DepartmentDetailsViewModel(department))
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new DeleteDepartmentCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
