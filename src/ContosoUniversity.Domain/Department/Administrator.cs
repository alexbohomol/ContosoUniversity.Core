namespace ContosoUniversity.Domain.Department
{
    public record Administrator(
        string FirstName,
        string LastName)
    {
        public string FullName  => string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName)
            ? $"{LastName}{FirstName}".Trim()
            : $"{LastName}, {FirstName}";
    }
}