using ContosoUniversity.DataAccess;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData
            {
                Instructors = db.Instructors
                                .Include(i => i.OfficeAssignment)
                                .Include(i => i.Courses.Select(c => c.Department))
                                .OrderBy(i => i.LastName)
            };

            // instructor was not selected -> no courses to show
            if (id == null)
            {
                return View(viewModel);
            }

            ViewBag.InstructorID = id.Value;
            viewModel.Courses = viewModel.Instructors
                                         .Where(i => i.Id == id.Value)
                                         .Single().Courses;

            // course was not selected -> no students to show
            if (courseID == null)
            {
                return View(viewModel);
            }

            ViewBag.CourseID = courseID.Value;
                
            //--> Lazy Loading
            //viewModel.Enrollments = viewModel.Courses
            //                                 .Where(x => x.Id == courseID)
            //                                 .Single().Enrollments;

            //--> Explicit loading
            var selectedCourse = viewModel.Courses
                                          .Where(x => x.Id == courseID)
                                          .Single();

            db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();

            foreach (Enrollment enrollment in selectedCourse.Enrollments)
            {
                db.Entry(enrollment).Reference(x => x.Student).Load();
            }

            viewModel.Enrollments = selectedCourse.Enrollments;

            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = db.Instructors
                                      .Include(i => i.OfficeAssignment)
                                      .SingleOrDefault(i => i.Id == id);
            
            if (instructor == null)
            {
                return HttpNotFound();
            }
            
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            var instructor = new Instructor
            {
                Courses = new List<Course>()
            };

            PopulateAssignedCourseData(instructor);

            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedCourseData(instructor);

            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = db.Instructors
                                      .Include(i => i.OfficeAssignment)
                                      .Include(i => i.Courses)
                                      .Where(i => i.Id == id)
                                      .Single();

            PopulateAssignedCourseData(instructor);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.Id));
            var viewModel = new List<AssignedCourseData>();

            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.Id)
                });
            }

            ViewBag.Courses = viewModel;
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorToUpdate = db.Instructors
                                       .Include(i => i.OfficeAssignment)
                                       .Include(i => i.Courses)
                                       .Where(i => i.Id == id)
                                       .Single();

            var updated = TryUpdateModel(instructorToUpdate, "", new string[] 
            {
                "LastName",
                "FirstMidName",
                "HireDate",
                "OfficeAssignment"
            });

            if (updated)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedCourseData(instructorToUpdate);

            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.Id));

            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = db.Instructors
                                      .Include(i => i.OfficeAssignment)
                                      .SingleOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var instructor = db.Instructors
                               .Include(i => i.OfficeAssignment)
                               .Where(i => i.Id == id)
                               .Single();
            
            db.Instructors.Remove(instructor);

            var department = db.Departments
                               .Where(d => d.InstructorId == id)
                               .SingleOrDefault();

            if (department != null)
            {
                department.InstructorId = null;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
