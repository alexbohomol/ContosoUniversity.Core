namespace Courses.Api.IntegrationTests;

using Models;

class Requests
{
    public class CreateCourse
    {
        public static CreateCourseRequest Valid => new(1234, "Computers", 5, Guid.NewGuid());
    }

    public class UpdateCourse
    {
        public static UpdateCourseRequest Valid => new("Quantum Computing", 3, Guid.NewGuid());
    }
}
