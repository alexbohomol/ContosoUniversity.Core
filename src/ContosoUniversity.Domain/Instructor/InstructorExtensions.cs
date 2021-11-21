namespace ContosoUniversity.Domain.Instructor;

public static class InstructorExtensions
{
    public static string FullName(this Instructor instructor)
    {
        return $"{instructor.LastName}, {instructor.FirstName}";
    }
}