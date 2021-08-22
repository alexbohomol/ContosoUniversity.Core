namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services.Courses.Commands;
    using Services.Courses.Queries;

    using ViewModels.Courses;

    public class CoursesController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly IMediator _mediator;

        public CoursesController(
            IDepartmentsRepository departmentsRepository,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsRepository = departmentsRepository;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new GetCoursesIndexQuery()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator
                .Send(new GetCourseDetailsQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return View(
                new CreateCourseForm(
                    await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new CreateCourseForm(
                        command,
                        await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
            }

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new GetCourseEditFormQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCourseCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var course = await _coursesRepository.GetById(command.Id, cancellationToken);

                return View(
                    new CourseEditForm(
                        command,
                        course.Code,
                        await _departmentsRepository.GetDepartmentNamesReference(cancellationToken)));
            }

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator
                .Send(new GetCourseDetailsQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCourseCommand(id));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseCredits(int? multiplier, CancellationToken cancellationToken)
        {
            if (multiplier.HasValue)
            {
                ViewData["RowsAffected"] = await _coursesRepository
                    .UpdateCourseCredits(multiplier.Value, cancellationToken);
            }

            return View();
        }
    }
}