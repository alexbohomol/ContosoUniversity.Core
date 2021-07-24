﻿namespace ContosoUniversity.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Commands.Courses;
    using Services.Queries.Courses;

    using ViewModels.Courses;

    public class CoursesController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;
        private readonly IMediator _mediator;

        public CoursesController(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new CoursesIndexQuery()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator
                .Send(new CourseDetailsQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create()
        {
            return View(
                new CreateCourseForm(
                    await _departmentsContext.GetDepartmentsNames()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseCommand command)
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
                        await _departmentsContext.GetDepartmentsNames()));
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

            var result = await _mediator.Send(new CourseEditFormQuery(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCourseCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var course = await _coursesRepository.GetById(command.Id);

                return View(
                    new CourseEditForm(
                        command,
                        course.Code,
                        await _departmentsContext.GetDepartmentsNames()));
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
                .Send(new CourseDetailsQuery(id.Value));

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
        public async Task<IActionResult> UpdateCourseCredits(int? multiplier)
        {
            if (multiplier.HasValue)
            {
                ViewData["RowsAffected"] = await _coursesRepository
                    .UpdateCourseCredits(multiplier.Value);
            }

            return View();
        }
    }
}