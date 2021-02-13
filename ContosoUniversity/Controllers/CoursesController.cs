﻿namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Contexts;
    using Data.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using Services;

    using ViewModels.Courses;

    public class CoursesController : Controller
    {
        private readonly SchoolContext _schoolContext;
        private readonly CoursesContext _coursesContext;

        public CoursesController(SchoolContext schoolContext, CoursesContext coursesContext)
        {
            _schoolContext = schoolContext;
            _coursesContext = coursesContext;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _coursesContext.Courses.AsNoTracking().ToListAsync();

            var departmentNames = await _schoolContext.Departments
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

        public async Task<IActionResult> Details(Guid? id)
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

            var department = await _schoolContext.Departments
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

        public async Task<IActionResult> Create()
        {
            return View(new CourseCreateForm
            {
                DepartmentsSelectList = await CreateDepartmentsDropDownList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateForm form)
        {
            if (ModelState.IsValid)
            {
                _coursesContext.Add(new Course
                {
                    CourseCode = form.CourseCode,
                    Title = form.Title,
                    Credits = form.Credits,
                    DepartmentExternalId = form.DepartmentId,
                    ExternalId = Guid.NewGuid()
                });
                await _coursesContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            form.DepartmentsSelectList = await CreateDepartmentsDropDownList(form.DepartmentId);
            return View(form);
        }

        public async Task<IActionResult> Edit(Guid? id)
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

            return View(new CourseEditForm
            {
                CourseCode = course.CourseCode,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentExternalId,
                DepartmentsSelectList = await CreateDepartmentsDropDownList(course.DepartmentExternalId)
            });
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CourseEditForm form)
        {
            if (form is null || ModelState.IsValid is false)
            {
                return BadRequest();
            }

            var courseToUpdate = await _coursesContext.Courses
                .FirstOrDefaultAsync(c => c.ExternalId == form.Id);

            if (await TryUpdateModelAsync(courseToUpdate, "",
                c => c.Credits,
                c => c.DepartmentExternalId,
                c => c.Title))
            {
                try
                {
                    await _coursesContext.SaveChangesAsync();
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
                    from d in _schoolContext.Departments
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

            var department = await _schoolContext.Departments
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
            var relatedAssignments = await _schoolContext.CourseAssignments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _schoolContext.CourseAssignments.RemoveRange(relatedAssignments);

            /*
             * remove related enrollments
             */
            var relatedEnrollments = await _schoolContext.Enrollments
                .Where(x => x.CourseExternalId == course.ExternalId)
                .ToArrayAsync();
            _schoolContext.Enrollments.RemoveRange(relatedEnrollments);

            _coursesContext.Courses.Remove(course);
            await _schoolContext.SaveChangesAsync();
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