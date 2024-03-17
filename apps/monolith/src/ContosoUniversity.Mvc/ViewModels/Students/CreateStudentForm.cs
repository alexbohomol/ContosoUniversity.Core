namespace ContosoUniversity.Mvc.ViewModels.Students;

public record CreateStudentForm
{
    public CreateStudentRequest Request { get; init; } = new();
}
