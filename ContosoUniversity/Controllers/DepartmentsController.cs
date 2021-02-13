namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Contexts;
    using Data.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using ViewModels.Departments;

    public class DepartmentsController : Controller
    {
        private const string ErrMsgConcurrentUpdate = "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, click the Save button again. Otherwise click the Back to List hyperlink.";

        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Administrator)
                .ToListAsync();

            return View(departments.Select(x => new DepartmentListItemViewModel
            {
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                Administrator = x.Administrator?.FullName,
                ExternalId = x.ExternalId
            }));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FromSqlInterpolated($"SELECT * FROM [dpt].Department WHERE ExternalId = {id}")
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return View(new DepartmentDetailsViewModel
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = department.Administrator?.FullName,
                ExternalId = department.ExternalId
            });
        }

        public IActionResult Create()
        {
            return View(new DepartmentCreateForm
            {
                StartDate = DateTime.Now,
                InstructorsDropDown = GetInstructorSelectList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateForm form)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Department
                {
                    Name = form.Name,
                    Budget = form.Budget,
                    StartDate = form.StartDate,
                    InstructorId = form.InstructorId,
                    ExternalId = Guid.NewGuid()
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            form.InstructorsDropDown = GetInstructorSelectList(form.InstructorId);
            return View(form);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(new DepartmentEditForm
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                InstructorId = department.InstructorId,
                ExternalId = department.ExternalId,
                RowVersion = department.RowVersion,
                InstructorsDropDown = GetInstructorSelectList(department.InstructorId)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentEditForm form)
        {
            if (form == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.ExternalId == form.ExternalId);

            if (departmentToUpdate == null)
            {
                var deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewData["InstructorsDropDown"] = GetInstructorSelectList(deletedDepartment.InstructorId);
                return View(form);
            }

            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = form.RowVersion;

            if (await TryUpdateModelAsync(
                departmentToUpdate,
                "",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department) exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department) databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }

                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                        }

                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
                        }

                        if (databaseValues.InstructorId != clientValues.InstructorId)
                        {
                            var databaseInstructor =
                                await _context.Instructors.FirstOrDefaultAsync(i =>
                                    i.Id == databaseValues.InstructorId);
                            ModelState.AddModelError("InstructorId", $"Current value: {databaseInstructor?.FullName}");
                        }

                        ModelState.AddModelError(string.Empty, ErrMsgConcurrentUpdate);
                        departmentToUpdate.RowVersion = databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }

            form.InstructorsDropDown = GetInstructorSelectList(departmentToUpdate.InstructorId);
            return View(form);
        }

        public async Task<IActionResult> Delete(Guid? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
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
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.ExternalId == id);
            if (department != null)
            {
                try
                {
                    var relatedCourses = await _context.Courses.Where(x => x.DepartmentExternalId == id).ToArrayAsync();
                    var relatedCoursesIds = relatedCourses.Select(x => x.ExternalId).ToArray();

                    /*
                     * remove related assignments
                     */
                    var relatedAssignments = await _context.CourseAssignments
                        .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                        .ToArrayAsync();
                    _context.CourseAssignments.RemoveRange(relatedAssignments);

                    /*
                     * remove related enrollments
                     */
                    var relatedEnrollments = await _context.Enrollments
                        .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                        .ToArrayAsync();
                    _context.Enrollments.RemoveRange(relatedEnrollments);

                    /*
                     * remove related courses
                     */
                    _context.Courses.RemoveRange(relatedCourses);

                    /*
                     * remove department
                     */
                    _context.Departments.Remove(department);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction(nameof(Delete), new {concurrencyError = true, id = department.ExternalId});
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private SelectList GetInstructorSelectList(int? departmentInstructorId = null)
        {
            return new(
                _context.Instructors,
                nameof(Instructor.Id),
                nameof(Instructor.FullName),
                departmentInstructorId);
        }
    }
}