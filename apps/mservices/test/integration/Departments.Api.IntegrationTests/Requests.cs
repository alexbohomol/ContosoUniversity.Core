namespace Departments.Api.IntegrationTests;

using Models;

class Requests
{
    public class Create
    {
        public class Department
        {
            public static CreateDepartmentRequest Valid => new(
                "Computers",
                1000000,
                DateTime.Now,
                Guid.Empty);
        }

        public class Instructor
        {
            public static CreateInstructorRequest Valid => new(
                "LastName",
                "FirstName",
                DateTime.Now,
                [Guid.NewGuid()],
                "Location");
        }
    }

    public class Update
    {
        public class Department
        {
            public static UpdateDepartmentRequest Valid => new(
                "Quantum Computers",
                2000000,
                DateTime.Now,
                Guid.NewGuid());
        }

        public class Instructor
        {
            public static UpdateInstructorRequest Valid => new(
                "Updated LastName",
                "Updated FirstName",
                DateTime.Now,
                [Guid.NewGuid()],
                "Updated Location");
        }
    }
}
