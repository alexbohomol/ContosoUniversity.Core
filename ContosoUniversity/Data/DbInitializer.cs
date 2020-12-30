namespace ContosoUniversity.Data
{
    using System;
    using System.Linq;

    using Models;

    public static class DbInitializer
    {
        public static void EnsureInitialized(SchoolContext context)
        {
            // Look for any students.
            if (context.Students.Any())
            {
                return; // DB has been seeded
            }

            var students = new[]
            {
                new Student
                {
                    FirstMidName = "Carson", 
                    LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2010-09-01")
                },
                new Student
                {
                    FirstMidName = "Meredith", 
                    LastName = "Alonso",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstMidName = "Arturo", 
                    LastName = "Anand",
                    EnrollmentDate = DateTime.Parse("2013-09-01")
                },
                new Student
                {
                    FirstMidName = "Gytis", 
                    LastName = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstMidName = "Yan", 
                    LastName = "Li",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstMidName = "Peggy", 
                    LastName = "Justice",
                    EnrollmentDate = DateTime.Parse("2011-09-01")
                },
                new Student
                {
                    FirstMidName = "Laura", 
                    LastName = "Norman",
                    EnrollmentDate = DateTime.Parse("2013-09-01")
                },
                new Student
                {
                    FirstMidName = "Nino", 
                    LastName = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01")
                }
            };

            context.Students.AddRange(students);

            context.SaveChanges();

            var instructors = new[]
            {
                new Instructor
                {
                    FirstMidName = "Kim", 
                    LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11")
                },
                new Instructor
                {
                    FirstMidName = "Fadi", 
                    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06")
                },
                new Instructor
                {
                    FirstMidName = "Roger", 
                    LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01")
                },
                new Instructor
                {
                    FirstMidName = "Candace", 
                    LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15")
                },
                new Instructor
                {
                    FirstMidName = "Roger", 
                    LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12")
                }
            };

            context.Instructors.AddRange(instructors);

            context.SaveChanges();

            var departments = new[]
            {
                new Department
                {
                    Name = "English", 
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new Department
                {
                    Name = "Mathematics", 
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new Department
                {
                    Name = "Engineering", 
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                },
                new Department
                {
                    Name = "Economics", 
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                }
            };

            context.Departments.AddRange(departments);

            context.SaveChanges();

            var courses = new[]
            {
                new Course
                {
                    CourseCode = 1050, 
                    Title = "Chemistry", 
                    Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Engineering").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 4022, 
                    Title = "Microeconomics", 
                    Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Economics").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 4041, 
                    Title = "Macroeconomics", 
                    Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Economics").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 1045, 
                    Title = "Calculus", 
                    Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 3141, 
                    Title = "Trigonometry", 
                    Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 2021, 
                    Title = "Composition", 
                    Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "English").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
                new Course
                {
                    CourseCode = 2042, 
                    Title = "Literature", 
                    Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "English").DepartmentID,
                    UniqueId = Guid.NewGuid()
                },
            };

            context.Courses.AddRange(courses);

            context.SaveChanges();

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
                },
            };

            context.OfficeAssignments.AddRange(officeAssignments);

            context.SaveChanges();

            var courseInstructors = new[]
            {
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Chemistry").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Chemistry").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Microeconomics").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Macroeconomics").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Calculus").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Trigonometry").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Composition").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new CourseAssignment
                {
                    CourseUid = courses.Single(c => c.Title == "Literature").UniqueId,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
            };

            context.CourseAssignments.AddRange(courseInstructors);

            context.SaveChanges();

            var enrollments = new[]
            {
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseUid = courses.Single(c => c.Title == "Chemistry").UniqueId,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseUid = courses.Single(c => c.Title == "Microeconomics").UniqueId,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseUid = courses.Single(c => c.Title == "Macroeconomics").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseUid = courses.Single(c => c.Title == "Calculus").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseUid = courses.Single(c => c.Title == "Trigonometry").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseUid = courses.Single(c => c.Title == "Composition").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseUid = courses.Single(c => c.Title == "Chemistry").UniqueId
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseUid = courses.Single(c => c.Title == "Microeconomics").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseUid = courses.Single(c => c.Title == "Chemistry").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Li").Id,
                    CourseUid = courses.Single(c => c.Title == "Composition").UniqueId,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Justice").Id,
                    CourseUid = courses.Single(c => c.Title == "Literature").UniqueId,
                    Grade = Grade.B
                }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments
                    .SingleOrDefault(s => 
                        s.Student.Id == e.StudentID &&
                        s.CourseUid == e.CourseUid);
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }

            context.SaveChanges();
        }
    }
}