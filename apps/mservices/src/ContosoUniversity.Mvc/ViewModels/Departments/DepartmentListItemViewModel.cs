namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.ComponentModel.DataAnnotations;

using global::Departments.Core.Projections;

public class DepartmentListItemViewModel
{
    public DepartmentListItemViewModel(Department department)
    {
        ArgumentNullException.ThrowIfNull(department, nameof(department));

        Name = department.Name;
        Budget = department.Budget;
        StartDate = department.StartDate;
        Administrator = department.AdministratorFullName;
        ExternalId = department.ExternalId;
    }

    public string Name { get; }

    public decimal Budget { get; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; }

    [Display(Name = "Administrator")] public string Administrator { get; }

    public Guid ExternalId { get; }
}
