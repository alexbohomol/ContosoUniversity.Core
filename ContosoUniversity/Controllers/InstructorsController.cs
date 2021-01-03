namespace ContosoUniversity.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using ViewModels;

    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(Guid? id, Guid? courseExternalId)
        {
            var viewModel = new InstructorIndexData
            {
                Instructors = await _context.Instructors
                    .Include(i => i.OfficeAssignment)
                    .Include(i => i.CourseAssignments)
                    .OrderBy(i => i.LastName)
                    .ToListAsync(),

                CoursesReference = await _context.Courses
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.ExternalId)
            };

            if (id is not null)
            {
                ViewData["selectedInstructorExternalId"] = id.Value;
                var instructor = viewModel.Instructors.Single(i => i.ExternalId == id.Value);
                var instructorCourses = instructor.CourseAssignments.Select(x => x.CourseExternalId);
                var courses = _context.Courses
                    .Where(x => instructorCourses.Contains(x.ExternalId));
                viewModel.SelectedInstructorCourses = courses;
                viewModel.DepartmentNamesReference = await _context.Departments
                    .Where(x => courses.Select(_ => _.DepartmentExternalId).Contains(x.ExternalId))
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.ExternalId, x => x.Name);
            }

            if (courseExternalId is not null)
            {
                ViewData["selectedCourseExternalId"] = courseExternalId.Value;
                viewModel.SelectedCourseEnrollments = _context.Enrollments
                    .Include(x => x.Student)
                    .Where(x => x.CourseExternalId == courseExternalId);
            }

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ExternalId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName,HireDate,LastName,OfficeAssignment")]
            Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var courseUid in selectedCourses)
                {
                    instructor.CourseAssignments.Add(new CourseAssignment
                    {
                        InstructorId = instructor.Id,
                        CourseExternalId = Guid.Parse(courseUid)
                    });
                }
            }

            if (ModelState.IsValid)
            {
                instructor.ExternalId = Guid.NewGuid();
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<Guid>(instructor.CourseAssignments.Select(c => c.CourseExternalId));

            ViewData["Courses"] = allCourses.Select(course => new AssignedCourseData
            {
                CourseCode = course.CourseCode,
                CourseExternalId = course.ExternalId,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.ExternalId)
            }).ToList();
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .FirstOrDefaultAsync(m => m.ExternalId == id);

            if (await TryUpdateModelAsync(
                instructorToUpdate,
                "",
                i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                    instructorToUpdate.OfficeAssignment = null;
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
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

            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHs = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<Guid>
                (instructorToUpdate.CourseAssignments.Select(c => c.CourseExternalId));
            foreach (var course in _context.Courses)
                if (selectedCoursesHs.Contains(course.ExternalId.ToString()))
                {
                    if (!instructorCourses.Contains(course.ExternalId))
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                        {
                            InstructorId = instructorToUpdate.Id,
                            CourseExternalId = course.ExternalId
                        });
                }
                else
                {
                    if (instructorCourses.Contains(course.ExternalId))
                    {
                        var courseToRemove =
                            instructorToUpdate.CourseAssignments.FirstOrDefault(i => i.CourseExternalId == course.ExternalId);
                        if (courseToRemove != null)
                            _context.Remove(courseToRemove);
                    }
                }
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ExternalId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ExternalId == id);

            var departments = await _context.Departments
                .Where(d => d.InstructorId == instructor.Id)
                .ToListAsync();

            departments.ForEach(d => d.InstructorId = null);

            _context.Instructors.Remove(instructor);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}