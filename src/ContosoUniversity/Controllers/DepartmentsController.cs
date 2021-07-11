namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Data.Departments;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Commands.Departments;
    using Services.Queries.Departments;

    using ViewModels;
    using ViewModels.Departments;

    public class DepartmentsController : Controller
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IMediator _mediator;

        public DepartmentsController(
            DepartmentsContext departmentsContext,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new QueryDepartmentsIndex()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator
                .Send(new QueryDepartmentDetails(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create()
        {
            return View(new DepartmentCreateForm
            {
                StartDate = DateTime.Now,
                InstructorsDropDown = (await _departmentsContext.GetInstructorsNames()).ToSelectList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateForm command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new DepartmentCreateForm(
                        command,
                        await _departmentsContext.GetInstructorsNames()));
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

            var result = await _mediator.Send(new QueryDepartmentEditForm(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentEditForm command)
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
                        await _departmentsContext.GetInstructorsNames()));
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
                .Send(new QueryDepartmentDetails(id.Value));

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