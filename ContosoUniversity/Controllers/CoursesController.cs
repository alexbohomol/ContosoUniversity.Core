namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Courses;
    using Data.Departments;
    using Data.Students;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services;
    using Services.Commands.Courses;
    using Services.Queries.Courses;

    using ViewModels.Courses;

    public class CoursesController : Controller
    {
        private readonly CoursesContext _coursesContext;
        private readonly DepartmentsContext _departmentsContext;
        private readonly IMediator _mediator;
        private readonly StudentsContext _studentsContext;

        public CoursesController(
            DepartmentsContext departmentsContext,
            CoursesContext coursesContext,
            StudentsContext studentsContext,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _coursesContext = coursesContext;
            _studentsContext = studentsContext;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new QueryCoursesIndex()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var result = await _mediator
                .Send(new QueryCourseDetails(id.Value));

            return result is not null
                ? View(result)
                : NotFound();
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreateCourseForm
            {
                DepartmentsSelectList = await _departmentsContext.ToDepartmentsDropDownList()
            });
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
                return View(new CreateCourseForm
                {
                    CourseCode = command.CourseCode,
                    Title = command.Title,
                    Credits = command.Credits,
                    DepartmentId = command.DepartmentId,
                    DepartmentsSelectList = await _departmentsContext.ToDepartmentsDropDownList(command.DepartmentId)
                });
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

            var result = await _mediator.Send(new QueryCourseEditForm(id.Value));

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
                return View(new EditCourseForm
                {
                    Title = command.Title,
                    Credits = command.Credits,
                    DepartmentId = command.DepartmentId,
                    DepartmentsSelectList = await _departmentsContext.ToDepartmentsDropDownList(command.DepartmentId)
                });
            }

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _coursesContext.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (course == null)
            {
                return NotFound();
            }

            var department = await _departmentsContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == course.DepartmentExternalId);

            /*
             * TODO: missing context boundary check when department is null
             */

            return View(new CourseDetailsViewModel
            {
                CourseCode = course.CourseCode,
                Title = course.Title,
                Credits = course.Credits,
                Department = department.Name,
                Id = course.ExternalId
            });
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _coursesContext.Courses.FirstOrDefaultAsync(x => x.ExternalId == id);
            if (course is null)
            {
                return NotFound();
            }

            /*
             * remove related assignments
             */
            var relatedAssignments = await _departmentsContext.CourseAssignments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

            /*
             * remove related enrollments
             */
            var relatedEnrollments = await _studentsContext.Enrollments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _studentsContext.Enrollments.RemoveRange(relatedEnrollments);

            _coursesContext.Courses.Remove(course);
            await _departmentsContext.SaveChangesAsync();
            await _studentsContext.SaveChangesAsync();
            await _coursesContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseCredits(int? multiplier)
        {
            if (multiplier is not null)
            {
                ViewData["RowsAffected"] =
                    await _coursesContext.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}");
            }

            return View();
        }
    }
}