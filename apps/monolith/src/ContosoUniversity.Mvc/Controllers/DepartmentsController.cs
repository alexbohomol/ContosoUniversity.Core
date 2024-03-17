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

using ViewModels.Departments;

public class DepartmentsController(IMediator mediator) : Controller
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

    public async Task<IActionResult> Create(
        [FromServices] IInstructorsRoRepository repository,
        CancellationToken cancellationToken)
    {
        Dictionary<Guid, string> instructorNames = await repository
            .GetInstructorNamesReference(cancellationToken);

        return View(new CreateDepartmentForm(instructorNames));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateDepartmentRequest request,
        [FromServices] IInstructorsRoRepository repository,
        [FromServices] CreateDepartmentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Dictionary<Guid, string> instructorNames = await repository
                .GetInstructorNamesReference(cancellationToken);

            return View(new CreateDepartmentForm(instructorNames));
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
            : View(new EditDepartmentForm(instructorsReference)
            {
                Request = new EditDepartmentRequest(department)
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditDepartmentRequest request,
        [FromServices] IInstructorsRoRepository repository,
        [FromServices] EditDepartmentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            Dictionary<Guid, string> instructorNames = await repository
                .GetInstructorNamesReference(cancellationToken);

            return View(new EditDepartmentForm(instructorNames)
            {
                Request = request
            });
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
