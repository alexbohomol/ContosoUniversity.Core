namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services.Commands.Students;
    using Services.Queries.Students;

    using ViewModels.Students;

    public class StudentsController : Controller
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(StudentsIndexQuery request)
        {
            return View(await _mediator.Send(request));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(new StudentDetailsQuery(id.Value));
            
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
        public async Task<IActionResult> Create(CreateStudentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command as CreateStudentForm);
            }

            await _mediator.Send(command);
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new StudentEditFormQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStudentCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(command as EditStudentForm);
            }

            await _mediator.Send(command);
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new StudentDeletePageQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteStudentCommand(id));

            return RedirectToAction(nameof(Index));
        }
    }
}