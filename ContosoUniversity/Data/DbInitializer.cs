namespace ContosoUniversity.Data
{
    using System;
    using System.IO;
    using System.Linq;

    using Courses;
    using Courses.Models;

    using Departments;
    using Departments.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Students;
    using Students.Models;

    class CourseConfig
    {
        public int CourseCode { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
    }

    class DepartmentConfig
    {
        public string Name { get; set; }
        public int Budget { get; set; }
        public DateTime StartDate { get; set; }
        public string Administrator { get; set; }
    }

    class StudentConfig
    {
        public DateTime EnrollmentDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class InstructorConfig
    {
        public DateTime HireDate { get; set; }
        public string Location { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class CourseAssignmentConfig
    {
        public string Instructor { get; set; }
        public int Course { get; set; }
    }

    class EnrollmentConfig
    {
        public string Student { get; set; }
        public int Course { get; set; }
        public string Grade { get; set; }
    }

    public static class DbInitializer
    {
        public static void EnsureInitialized(
            DepartmentsContext departmentsContext,
            CoursesContext coursesContext,
            StudentsContext studentsContext)
        {
            // Check seed status
            if (coursesContext.Courses.Any() 
                || studentsContext.Students.Any()
                || departmentsContext.Departments.Any()
                || departmentsContext.Instructors.Any())
            {
                return; // DB has been seeded
            }
            
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ContosoUniversity"))
                .AddJsonFile("appsettings.Seed.json")
                .Build();

            var courseConfigs = config
                .GetSection("CoursesContext:Courses")
                .Get<CourseConfig[]>();
            
            coursesContext.Courses.AddRange(courseConfigs.Select(x => new Course
            {
                CourseCode = x.CourseCode,
                Title = x.Title,
                Credits = x.Credits,
                ExternalId = Guid.NewGuid()
                // DepartmentExternalId = 
            }));

            coursesContext.SaveChanges();

            var departmentConfigs = config
                .GetSection("DepartmentsContext:Departments")
                .Get<DepartmentConfig[]>();
            
            departmentsContext.Departments.AddRange(departmentConfigs.Select(x => new Department
            {
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                ExternalId = Guid.NewGuid()
                // InstructorId = 
            }));
            
            departmentsContext.SaveChanges();
            
            foreach (var course in coursesContext.Courses)
            {
                var department = courseConfigs.Single(x => x.CourseCode == course.CourseCode).Department;
                course.DepartmentExternalId = departmentsContext.Departments.Single(x => x.Name == department).ExternalId;
            }
            
            coursesContext.SaveChanges();

            var studentConfigs = config
                .GetSection("StudentsContext:Students")
                .Get<StudentConfig[]>();
            
            studentsContext.Students.AddRange(studentConfigs.Select(x => new Student
            {
                FirstMidName = x.FirstName,
                LastName = x.LastName,
                EnrollmentDate = x.EnrollmentDate,
                ExternalId = Guid.NewGuid()
            }));

            studentsContext.SaveChanges();

            var instructorConfigs = config
                .GetSection("DepartmentsContext:Instructors")
                .Get<InstructorConfig[]>();

            var instructors = instructorConfigs.Select(x => new Instructor
            {
                FirstMidName = x.FirstName,
                LastName = x.LastName,
                HireDate = x.HireDate,
                OfficeAssignment = string.IsNullOrWhiteSpace(x.Location)
                    ? null
                    : new OfficeAssignment
                    {
                        Location = x.Location
                    },
                ExternalId = Guid.NewGuid()
            }).ToArray();
            
            departmentsContext.Instructors.AddRange(instructors);

            departmentsContext.SaveChanges();
            
            foreach (var department in departmentsContext.Departments)
            {
                string administrator = departmentConfigs.Single(x => x.Name == department.Name).Administrator;
                department.Administrator = instructors.FirstOrDefault(x => x.FullName == administrator);
            }
            
            departmentsContext.SaveChanges();

            var courseAssignmentConfigs = config
                .GetSection("DepartmentsContext:CourseAssignments")
                .Get<CourseAssignmentConfig[]>();

            var courses = coursesContext.Courses.AsNoTracking().ToArray();
            
            departmentsContext.CourseAssignments.AddRange(courseAssignmentConfigs.Select(x => new CourseAssignment
            {
                CourseExternalId = courses.Single(c => c.CourseCode == x.Course).ExternalId,
                InstructorId = instructors.Single(i => i.FullName == x.Instructor).Id
            }));

            departmentsContext.SaveChanges();

            var enrollmentConfigs = config
                .GetSection("StudentsContext:Enrollments")
                .Get<EnrollmentConfig[]>();

            var students = studentsContext.Students.AsNoTracking().ToArray();

            studentsContext.Enrollments.AddRange(enrollmentConfigs.Select(x => new Enrollment
            {
                CourseExternalId = courses.Single(c => c.CourseCode == x.Course).ExternalId,
                StudentId = students.Single(s => s.FullName == x.Student).Id,
                Grade = Enum.TryParse<Grade>(x.Grade, true, out var grade)
                    ? grade
                    : null
            }));

            studentsContext.SaveChanges();
        }
    }
}