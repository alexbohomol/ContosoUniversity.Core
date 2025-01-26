namespace Departments.Api.IntegrationTests;

using Models;

class Requests
{
    public class CreateDepartment
    {
        public static CreateDepartmentRequest Valid => new("Computers", 1000000, DateTime.Now, Guid.Empty);
    }

    public class UpdateDepartment
    {
        public static EditDepartmentRequest Valid => new("Quantum Computers", 2000000, DateTime.Now, Guid.NewGuid(),
            Guid.NewGuid(),
            Convert.FromBase64String("other version"));
    }

    public class CreateInstructor
    {
        public static CreateInstructorRequest Valid => new("LastName", "FirstName", DateTime.Now,
            [ Guid.NewGuid() ],
            "Location");
    }

    public class UpdateInstructor
    {
        public static EditInstructorRequest Valid => new(Guid.Empty, "Updated LastName", "Updated FirstName", DateTime.Now,
            [ Guid.NewGuid() ],
            "Updated Location");
    }
}
