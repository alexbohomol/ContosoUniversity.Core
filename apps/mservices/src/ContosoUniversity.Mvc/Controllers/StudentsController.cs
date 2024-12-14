namespace ContosoUniversity.Mvc.Controllers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Students.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SharedKernel.Paging;

using Students.Core.Handlers.Commands;
using Students.Core.Projections;

using ViewModels.Students;

public class StudentsController(IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(GetStudentsIndexQuery request, CancellationToken cancellationToken)
    {
        if (request.SearchString is not null)
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
        if (id is null)
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
        return View(new CreateStudentForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateStudentRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(new CreateStudentForm
            {
                Request = request
            });
        }

        await mediator.Send(
            new CreateStudentCommand(
                request.EnrollmentDate,
                request.LastName,
                request.FirstName),
            cancellationToken);

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
            : View(new EditStudentForm
            {
                Request = new EditStudentRequest(student)
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditStudentRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(new EditStudentForm
            {
                Request = request
            });
        }

        await mediator.Send(new EditStudentCommand
        {
            ExternalId = request.ExternalId,
            LastName = request.LastName,
            FirstName = request.FirstName,
            EnrollmentDate = request.EnrollmentDate
        }, cancellationToken);

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
