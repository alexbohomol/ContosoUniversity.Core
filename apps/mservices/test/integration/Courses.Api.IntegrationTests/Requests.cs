namespace Courses.Api.IntegrationTests;

using Models;

class Requests
{
    public class Create
    {
        public class Course
        {
            public static CreateCourseRequest Valid => new(1234, "Computers", 5, Guid.NewGuid());
        }
    }

    public class Update
    {
        public class Course
        {
            public static UpdateCourseRequest Valid => new("Quantum Computing", 3, Guid.NewGuid());
        }
    }
}
