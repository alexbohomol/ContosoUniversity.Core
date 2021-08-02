namespace ContosoUniversity.Domain.Instructor
{
    public static class InstructorExtensions
    {
        public static string FullName(this Instructor instructor) => $"{instructor.LastName}, {instructor.FirstName}"; 
    }
}