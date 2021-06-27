namespace ContosoUniversity.ViewModels
{
    using Domain.Student;

    public static class DomainExtensions
    {
        public static string ToDisplayString(this Grade grade)
        {
            return grade switch
            {
                Grade.Undefined => "No grade",
                var regular => regular.ToString()
            };
        }
    }
}