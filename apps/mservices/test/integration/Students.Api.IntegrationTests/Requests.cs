namespace Students.Api.IntegrationTests;

using Models;

class Requests
{
    public class CreateStudent
    {
        public static CreateStudentRequest Valid => new(DateTime.Now, "LastName", "FirstName");
    }

    public class UpdateStudent
    {
        public static EditStudentRequest Valid => new(DateTime.Now, "LastName Updated", "FirstName Updated", Guid.Empty);
    }
}
