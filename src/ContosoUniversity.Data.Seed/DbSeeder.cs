namespace ContosoUniversity.Data.Seed
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

    using Models;

    using Students;
    using Students.Models;

    internal static class DbSeeder
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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Seed.json")
                .Build();

            var courseConfigs = config
                .GetSection("CoursesContext:Courses")
                .Get<CourseSeedModel[]>();

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
                .Get<DepartmentSeedModel[]>();

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

            var students = config
                .GetSection("StudentsContext:Students")
                .Get<StudentSeedModel[]>()
                .Select(x => new Student
                {
                    FirstMidName = x.FirstName,
                    LastName = x.LastName,
                    EnrollmentDate = x.EnrollmentDate,
                    ExternalId = Guid.NewGuid()
                }).ToArray();

            studentsContext.Students.AddRange(students);

            studentsContext.SaveChanges();

            var instructorConfigs = config
                .GetSection("DepartmentsContext:Instructors")
                .Get<InstructorSeedModel[]>();

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
                var administrator = departmentConfigs.Single(x => x.Name == department.Name).Administrator;
                department.InstructorId = instructors.FirstOrDefault(x => x.FullName == administrator)?.Id;
            }

            departmentsContext.SaveChanges();

            var courses = coursesContext.Courses.AsNoTracking().ToArray();

            departmentsContext.CourseAssignments.AddRange(config
                .GetSection("DepartmentsContext:CourseAssignments")
                .Get<CourseAssignmentSeedModel[]>()
                .Select(x => new CourseAssignment
                {
                    CourseExternalId = courses.Single(c => c.CourseCode == x.Course).ExternalId,
                    InstructorId = instructors.Single(i => i.FullName == x.Instructor).Id
                }));

            departmentsContext.SaveChanges();

            studentsContext.Enrollments.AddRange(config
                .GetSection("StudentsContext:Enrollments")
                .Get<EnrollmentSeedModel[]>()
                .Select(x => new Enrollment
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