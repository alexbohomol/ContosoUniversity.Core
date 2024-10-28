namespace ContosoUniversity.Mvc.ViewModels.Students;

public record EditStudentForm
{
    public EditStudentRequest Request { get; init; } = new();
}
