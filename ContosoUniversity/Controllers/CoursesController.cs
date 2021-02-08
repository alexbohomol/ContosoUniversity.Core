namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Services;

    using ViewModels.Courses;

    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.AsNoTracking().ToListAsync();

            var departmentNames = await _context.Departments
                .Where(x => courses.Select(_ => _.DepartmentExternalId).Distinct().Contains(x.ExternalId))
                .AsNoTracking()
                .ToDictionaryAsync(x => x.ExternalId, x => x.Name);

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames);

            return View(courses.Select(x => new CourseListItemViewModel
            {
                CourseCode = x.CourseCode,
                Title = x.Title,
                Credits = x.Credits,
                Department = departmentNames[x.DepartmentExternalId],
                Id = x.ExternalId
            }).ToList());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (course == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
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

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            return View(new CourseCreateForm
            {
                DepartmentsSelectList = await CreateDepartmentsDropDownList()
            });
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateForm form)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Course
                {
                    CourseCode = form.CourseCode,
                    Title = form.Title,
                    Credits = form.Credits,
                    DepartmentExternalId = form.DepartmentId,
                    ExternalId = Guid.NewGuid()
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            form.DepartmentsSelectList = await CreateDepartmentsDropDownList(form.DepartmentId);
            return View(form);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(new CourseEditForm
            {
                CourseCode = course.CourseCode,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentExternalId,
                DepartmentsSelectList = await CreateDepartmentsDropDownList(course.DepartmentExternalId)
            });
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CourseEditForm form)
        {
            if (form is null || ModelState.IsValid is false)
            {
                return BadRequest();
            }

            var courseToUpdate = await _context.Courses
                .FirstOrDefaultAsync(c => c.ExternalId == form.Id);

            if (await TryUpdateModelAsync(courseToUpdate, "",
                c => c.Credits,
                c => c.DepartmentExternalId,
                c => c.Title))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }

                return RedirectToAction(nameof(Index));
            }

            form.DepartmentsSelectList = await CreateDepartmentsDropDownList(courseToUpdate.DepartmentExternalId);
            return View(form);
        }

        private async Task<SelectList> CreateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departments = await (
                    from d in _context.Departments
                    orderby d.Name
                    select d)
                .AsNoTracking()
                .ToArrayAsync();

            return new SelectList(
                departments,
                nameof(Department.ExternalId),
                nameof(Department.Name),
                selectedDepartment);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (course == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
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

        // POST: Courses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.ExternalId == id);
            if (course is null)
            {
                return NotFound();
            }

            /*
             * remove related assignments
             */
            var relatedAssignments = await _context.CourseAssignments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _context.CourseAssignments.RemoveRange(relatedAssignments);

            /*
             * remove related enrollments
             */
            var relatedEnrollments = await _context.Enrollments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _context.Enrollments.RemoveRange(relatedEnrollments);

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
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
                    await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}");
            }

            return View();
        }
    }
}