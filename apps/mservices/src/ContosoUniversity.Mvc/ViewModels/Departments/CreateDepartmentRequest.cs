namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;

public record CreateDepartmentRequest
{
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; } = DateTime.Today;
    public Guid? AdministratorId { get; init; }
}
