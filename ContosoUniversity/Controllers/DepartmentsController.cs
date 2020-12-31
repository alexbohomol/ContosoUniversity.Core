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

    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Administrator)
                .ToListAsync();
            
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FromSqlInterpolated($"SELECT * FROM [dpt].Department WHERE DepartmentID = {id}")
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["InstructorID"] = GetInstructorSelectList();
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.UniqueId = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = GetInstructorSelectList(department.InstructorID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UniqueId == id);

            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = GetInstructorSelectList(department.InstructorID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.UniqueId == id);

            if (departmentToUpdate == null)
            {
                var deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewData["InstructorID"] = GetInstructorSelectList(deletedDepartment.InstructorID);
                return View(deletedDepartment);
            }

            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

            if (await TryUpdateModelAsync(
                departmentToUpdate,
                "",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

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
                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            Instructor databaseInstructor = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == databaseValues.InstructorID);
                            ModelState.AddModelError("InstructorID", $"Current value: {databaseInstructor?.FullName}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");
                        departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            ViewData["InstructorID"] = GetInstructorSelectList(departmentToUpdate.InstructorID);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(department);
        }
        
        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.UniqueId == id);
            if (department != null)
            {
                try
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = department.UniqueId });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetInstructorSelectList(int? departmentInstructorId = null) => new(
            _context.Instructors, 
            nameof(Instructor.Id), 
            nameof(Instructor.FullName), 
            departmentInstructorId);
    }
}
