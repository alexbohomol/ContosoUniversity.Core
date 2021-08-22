namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services.Departments.Commands;
    using Services.Departments.Queries;

    using ViewModels;
    using ViewModels.Departments;

    public class DepartmentsController : Controller
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly IMediator _mediator;

        public DepartmentsController(
            IInstructorsRepository instructorsRepository,
            IMediator mediator)
        {
            _instructorsRepository = instructorsRepository;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new GetDepartmentsIndexQuery()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator
                .Send(new GetDepartmentDetailsQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreateDepartmentForm
            {
                StartDate = DateTime.Now,
                InstructorsDropDown = (await _instructorsRepository.GetInstructorNamesReference()).ToSelectList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new CreateDepartmentForm(
                        command,
                        await _instructorsRepository.GetInstructorNamesReference()));
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

            var result = await _mediator.Send(new GetDepartmentEditFormQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartmentCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new DepartmentEditForm(
                        command,
                        await _instructorsRepository.GetInstructorNamesReference()));
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

            var result = await _mediator
                .Send(new GetDepartmentDetailsQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteDepartmentCommand(id));
            
            return RedirectToAction(nameof(Index));
        }
    }
}