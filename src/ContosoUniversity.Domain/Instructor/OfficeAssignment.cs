namespace ContosoUniversity.Domain.Instructor;

public record OfficeAssignment(string Title)
{
    public const int TitleMaxLength = 50;
}