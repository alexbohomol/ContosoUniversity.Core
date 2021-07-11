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

    using Services;
    using Services.Queries.Departments;

    using ViewModels;
    using ViewModels.Departments;

    public class DepartmentsController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;
        private readonly IStudentsRepository _studentsRepository;
        private readonly IMediator _mediator;

        public DepartmentsController(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository,
            IStudentsRepository studentsRepository,
            IMediator mediator)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
            _studentsRepository = studentsRepository;
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

        public async Task<IActionResult> Delete(Guid? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentsContext.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }

            return View(new DepartmentDeleteViewModel
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = department.Administrator?.FullName,
                ExternalId = department.ExternalId,
                RowVersion = department.RowVersion,
                ConcurrencyError = concurrencyError.GetValueOrDefault()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var department = await _departmentsContext.Departments.FirstOrDefaultAsync(x => x.ExternalId == id);
            if (department != null)
            {
                try
                {
                    var relatedCourses = await _coursesRepository.GetByDepartmentId(id);
                    var relatedCoursesIds = relatedCourses.Select(x => x.EntityId).ToArray();

                    /*
                     * remove related assignments
                     */
                    var relatedAssignments = await _departmentsContext.CourseAssignments
                        .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                        .ToArrayAsync();
                    _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

                    /*
                     * remove related enrollments (withdraw related students)
                     */
                    var students = await _studentsRepository.GetStudentsEnrolledForCourses(relatedCoursesIds);
                    foreach (var student in students)
                    {
                        var withdrawIds = relatedCoursesIds.Intersect(student.Enrollments.CourseIds);
                        student.WithdrawCourses(withdrawIds.ToArray());
                        await _studentsRepository.Save(student);
                    }

                    /*
                     * remove related courses
                     */
                    await _coursesRepository.Remove(relatedCoursesIds);

                    /*
                     * remove department
                     */
                    _departmentsContext.Departments.Remove(department);

                    await _departmentsContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction(nameof(Delete), new {concurrencyError = true, id = department.ExternalId});
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}