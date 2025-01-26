namespace Students.Api.IntegrationTests;

using Models;

class Requests
{
    public class Create
    {
        public class Student
        {
            public static CreateStudentRequest Valid => new(DateTime.Now, "LastName", "FirstName");
        }
    }

    public class Update
    {
        public class Student
        {
            public static UpdateStudentRequest Valid => new(DateTime.Now, "LastName Updated", "FirstName Updated");
        }
    }
}
