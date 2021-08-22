namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services.Instructors.Commands;
    using Services.Instructors.Queries;

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

        public async Task<IActionResult> Index(GetInstructorsIndexQuery request, CancellationToken cancellationToken)
        {
            return View(
                await _mediator.Send(
                    request,
                    cancellationToken));
        }

        public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(
                new GetInstructorDetailsQuery(id.Value),
                cancellationToken);
            
            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var courses = await _coursesRepository.GetAll(cancellationToken);
            
            return View(new CreateInstructorForm
            {
                HireDate = DateTime.Now,
                AssignedCourses = courses.ToAssignedCourseOptions()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstructorCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var courses = await _coursesRepository.GetAll(cancellationToken);
                
                return View(
                    new CreateInstructorForm(
                        command,
                        courses.ToAssignedCourseOptions()));
            }

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(
                new GetInstructorEditFormQuery(id.Value),
                cancellationToken);

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInstructorCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var courses = await _coursesRepository.GetAll(cancellationToken);
                
                return View(
                    new EditInstructorForm(
                        command,
                        courses.ToAssignedCourseOptions(/* instructor? */)));
            }

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(
                new GetInstructorDetailsQuery(id.Value),
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
                new DeleteInstructorCommand(id),
                cancellationToken);
            
            return RedirectToAction(nameof(Index));
        }
    }
}