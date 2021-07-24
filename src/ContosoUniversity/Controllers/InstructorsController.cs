namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Domain.Contracts;
    
    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services.Commands.Instructors;
    using Services.Queries.Instructors;

    using ViewModels;
    using ViewModels.Instructors;

    public class InstructorsController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMediator _mediator;

        public InstructorsController(
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(InstructorsIndexQuery request)
        {
            return View(await _mediator.Send(request));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(new InstructorDetailsQuery(id.Value));
            
            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreateInstructorForm
            {
                HireDate = DateTime.Now,
                AssignedCourses = (await _coursesRepository.GetAll()).ToAssignedCourseOptions()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstructorCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new CreateInstructorForm(
                        command,
                        (await _coursesRepository.GetAll()).ToAssignedCourseOptions()));
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

            var result = await _mediator.Send(new InstructorEditFormQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInstructorCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new EditInstructorForm(
                        command,
                        (await _coursesRepository.GetAll()).ToAssignedCourseOptions(/* instructor? */)));
            }

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(new InstructorDetailsQuery(id.Value));
            
            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteInstructorCommand(id));
            
            return RedirectToAction(nameof(Index));
        }
    }
}