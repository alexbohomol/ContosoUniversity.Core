using ContosoUniversity.DataAccess;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System.Data.Entity;
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
                return View(viewModel);

            ViewBag.InstructorID = id.Value;
            viewModel.Courses = viewModel.Instructors
                                         .Where(i => i.Id == id.Value)
                                         .Single().Courses;

            // course was not selected -> no students to show
            if (courseID == null)
                return View(viewModel);

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
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.OfficeAssignments, "InstructorId", "Location");
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.OfficeAssignments, "InstructorId", "Location", instructor.Id);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.OfficeAssignments, "InstructorId", "Location", instructor.Id);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.OfficeAssignments, "InstructorId", "Location", instructor.Id);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
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
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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
