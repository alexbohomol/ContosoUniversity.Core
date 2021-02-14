namespace ContosoUniversity.Data
{
    using System;
    using System.Linq;

    using Contexts;

    using Courses;
    using Courses.Models;

    using Models;

    using Students;
    using Students.Models;

    public static class DbInitializer
    {
        public static void EnsureInitialized(
            DepartmentsContext departmentsContext, 
            CoursesContext coursesContext,
            StudentsContext studentsContext)
        {
            // Look for any students.
            if (studentsContext.Students.Any())
            {
                return; // DB has been seeded
            }

            var students = new[]
            {
                new Student
                {
                    FirstMidName = "Carson",
                    LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2010-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Meredith",
                    LastName = "Alonso",
                    EnrollmentDate = DateTime.Parse("2012-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Arturo",
                    LastName = "Anand",
                    EnrollmentDate = DateTime.Parse("2013-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Gytis",
                    LastName = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2012-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Yan",
                    LastName = "Li",
                    EnrollmentDate = DateTime.Parse("2012-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Peggy",
                    LastName = "Justice",
                    EnrollmentDate = DateTime.Parse("2011-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Laura",
                    LastName = "Norman",
                    EnrollmentDate = DateTime.Parse("2013-09-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Student
                {
                    FirstMidName = "Nino",
                    LastName = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01"),
                    ExternalId = Guid.NewGuid()
                }
            };

            studentsContext.Students.AddRange(students);

            departmentsContext.SaveChanges();

            var instructors = new[]
            {
                new Instructor
                {
                    FirstMidName = "Kim",
                    LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11"),
                    ExternalId = Guid.NewGuid()
                },
                new Instructor
                {
                    FirstMidName = "Fadi",
                    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06"),
                    ExternalId = Guid.NewGuid()
                },
                new Instructor
                {
                    FirstMidName = "Roger",
                    LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01"),
                    ExternalId = Guid.NewGuid()
                },
                new Instructor
                {
                    FirstMidName = "Candace",
                    LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15"),
                    ExternalId = Guid.NewGuid()
                },
                new Instructor
                {
                    FirstMidName = "Roger",
                    LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12"),
                    ExternalId = Guid.NewGuid()
                }
            };

            departmentsContext.Instructors.AddRange(instructors);

            departmentsContext.SaveChanges();

            var departments = new[]
            {
                new Department
                {
                    Name = "English",
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id,
                    ExternalId = Guid.NewGuid()
                },
                new Department
                {
                    Name = "Mathematics",
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id,
                    ExternalId = Guid.NewGuid()
                },
                new Department
                {
                    Name = "Engineering",
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id,
                    ExternalId = Guid.NewGuid()
                },
                new Department
                {
                    Name = "Economics",
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id,
                    ExternalId = Guid.NewGuid()
                }
            };

            departmentsContext.Departments.AddRange(departments);

            departmentsContext.SaveChanges();

            var courses = new[]
            {
                new Course
                {
                    CourseCode = 1050,
                    Title = "Chemistry",
                    Credits = 3,
                    DepartmentExternalId = departments.Single(s => s.Name == "Engineering").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 4022,
                    Title = "Microeconomics",
                    Credits = 3,
                    DepartmentExternalId = departments.Single(s => s.Name == "Economics").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 4041,
                    Title = "Macroeconomics",
                    Credits = 3,
                    DepartmentExternalId = departments.Single(s => s.Name == "Economics").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 1045,
                    Title = "Calculus",
                    Credits = 4,
                    DepartmentExternalId = departments.Single(s => s.Name == "Mathematics").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 3141,
                    Title = "Trigonometry",
                    Credits = 4,
                    DepartmentExternalId = departments.Single(s => s.Name == "Mathematics").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 2021,
                    Title = "Composition",
                    Credits = 3,
                    DepartmentExternalId = departments.Single(s => s.Name == "English").ExternalId,
                    ExternalId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 2042,
                    Title = "Literature",
                    Credits = 4,
                    DepartmentExternalId = departments.Single(s => s.Name == "English").ExternalId,
                    ExternalId = Guid.NewGuid()
                }
            };

            coursesContext.Courses.AddRange(courses);

            departmentsContext.SaveChanges();

            var officeAssignments = new[]
            {
                new OfficeAssignment
                {
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id,
                    Location = "Smith 17"
                },
                new OfficeAssignment
                {
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id,
                    Location = "Gowan 27"
                },
                new OfficeAssignment
                {
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id,
                    Location = "Thompson 304"
                }
            };

            departmentsContext.OfficeAssignments.AddRange(officeAssignments);

            departmentsContext.SaveChanges();

            var courseInstructors = new[]
            {
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Chemistry").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Chemistry").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Microeconomics").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Macroeconomics").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Calculus").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Trigonometry").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Composition").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.Title == "Literature").ExternalId,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                }
            };

            departmentsContext.CourseAssignments.AddRange(courseInstructors);

            departmentsContext.SaveChanges();

            var enrollments = new[]
            {
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Chemistry").ExternalId,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Microeconomics").ExternalId,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Macroeconomics").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Calculus").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Trigonometry").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Composition").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Chemistry").ExternalId
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Microeconomics").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Chemistry").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Li").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Composition").ExternalId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Justice").Id,
                    CourseExternalId = courses.Single(c => c.Title == "Literature").ExternalId,
                    Grade = Grade.B
                }
            };

            foreach (var e in enrollments)
            {
                var enrollmentInDataBase = studentsContext.Enrollments
                    .SingleOrDefault(s =>
                        s.Student.Id == e.StudentId &&
                        s.CourseExternalId == e.CourseExternalId);
                if (enrollmentInDataBase == null)
                {
                    studentsContext.Enrollments.Add(e);
                }
            }

            departmentsContext.SaveChanges();
        }
    }
}