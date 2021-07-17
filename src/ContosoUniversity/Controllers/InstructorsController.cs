namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;
    
    using MediatR;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Commands.Instructors;
    using Services.Queries.Instructors;

    using ViewModels;
    using ViewModels.Instructors;

    public class InstructorsController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;
        private readonly IMediator _mediator;

        public InstructorsController(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(QueryInstructorsIndex request)
        {
            return View(await _mediator.Send(request));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _mediator.Send(new QueryInstructorDetails(id.Value));
            
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

            var result = await _mediator.Send(new QueryInstructorEditForm(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InstructorEditCommand command)
        {
            if (command is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(
                    new InstructorEditForm(
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

            var instructor = await _departmentsContext.Instructors
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(new InstructorDetailsViewModel
            {
                LastName = instructor.LastName,
                FirstName = instructor.FirstMidName,
                HireDate = instructor.HireDate,
                ExternalId = instructor.ExternalId
            });
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ExternalId == id);

            var departments = await _departmentsContext.Departments
                .Where(d => d.InstructorId == instructor.Id)
                .ToListAsync();

            departments.ForEach(d => d.InstructorId = null);

            _departmentsContext.Instructors.Remove(instructor);

            await _departmentsContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}