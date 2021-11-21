namespace ContosoUniversity.Domain.Student;

public static class StudentExtensions
{
    public static string FullName(this Student student)
    {
        return $"{student.FirstName}, {student.LastName}";
    }
}