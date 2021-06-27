namespace ContosoUniversity.Domain.Student
{
    public static class StudentExtensions
    {
        public static string FullName(this Student student) => $"{student.FirstName}, {student.LastName}";
    }
}