namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class DepartmentDetailsViewModel
{
    public DepartmentDetailsViewModel(Department department)
    {
        ArgumentNullException.ThrowIfNull(department, nameof(department));

        Name = department.Name;
        Budget = department.Budget;
        StartDate = department.StartDate;
        Administrator = department.AdministratorFullName;
        ExternalId = department.ExternalId;
    }

    public string Name { get; }

    [DataType(DataType.Currency)] public decimal Budget { get; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; }

    public string Administrator { get; }

    public Guid ExternalId { get; }
}