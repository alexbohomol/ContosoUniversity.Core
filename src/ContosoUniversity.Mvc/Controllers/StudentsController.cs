namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly.Paging;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Students.Commands;
using Application.Services.Students.Queries;
using Application.Services.Students.Validators;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Students;

public class StudentsController(IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(GetStudentsIndexQuery request, CancellationToken cancellationToken)
    {
        if (request.SearchString != null)
        {
            request.PageNumber = 1;
        }
        else
        {
            request.SearchString = request.CurrentFilter;
        }

        (PageInfo pageInfo, Student[] students) = await mediator.Send(request, cancellationToken);

        return View(new StudentIndexViewModel(request, pageInfo, students));
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null)
        {
            return NotFound();
        }

        (Student student, Dictionary<Guid, string> courseTitles) = await mediator.Send(
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
    public async Task<IActionResult> Create(
        CreateStudentRequest request,
        [FromServices] CreateStudentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(request as CreateStudentForm);
        }

        CreateStudentCommand command = new()
        {
            EnrollmentDate = request.EnrollmentDate,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        var result = validator.Validate(command);
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

        Student student = await mediator.Send(
            new GetStudentProjectionQuery(id.Value),
            cancellationToken);

        return student is null
            ? NotFound()
            : View(new EditStudentForm(student));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditStudentRequest request,
        [FromServices] EditStudentCommandValidator validator,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(request as EditStudentForm);
        }

        EditStudentCommand command = new()
        {
            ExternalId = request.ExternalId,
            LastName = request.LastName,
            FirstName = request.FirstName,
            EnrollmentDate = request.EnrollmentDate
        };
        var result = validator.Validate(command);
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

        Student student = await mediator.Send(
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
        await mediator.Send(
            new DeleteStudentCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
