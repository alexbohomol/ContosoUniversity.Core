namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;

using global::Departments.Core.Projections;

public record EditDepartmentRequest
{
    public EditDepartmentRequest(Department department)
    {
        Name = department.Name;
        Budget = department.Budget;
        StartDate = department.StartDate;
        AdministratorId = department.AdministratorId;
        ExternalId = department.ExternalId;
        // RowVersion = department.RowVersion,
    }

    public EditDepartmentRequest() { }

    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public Guid? AdministratorId { get; init; }
    public Guid ExternalId { get; init; }
    public byte[] RowVersion { get; init; }
}
