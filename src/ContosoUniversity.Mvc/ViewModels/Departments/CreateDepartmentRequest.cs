namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;

public class CreateDepartmentRequest
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public Guid? AdministratorId { get; set; }
}
