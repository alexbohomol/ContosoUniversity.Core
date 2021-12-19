namespace ContosoUniversity.ViewModels.Departments;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class DepartmentListItemViewModel
{
    public DepartmentListItemViewModel(Department department, Dictionary<Guid, string> instructorsReference)
    {
        ArgumentNullException.ThrowIfNull(department, nameof(department));
        ArgumentNullException.ThrowIfNull(instructorsReference, nameof(instructorsReference));

        Name = department.Name;
        Budget = department.Budget;
        StartDate = department.StartDate;
        Administrator = department.AdministratorId.HasValue
            ? instructorsReference.ContainsKey(department.AdministratorId.Value)
                ? instructorsReference[department.AdministratorId.Value]
                : string.Empty
            : string.Empty;
        ExternalId = department.ExternalId;
    }

    public string Name { get; }

    [DataType(DataType.Currency)] public decimal Budget { get; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; }

    [Display(Name = "Administrator")] public string Administrator { get; }

    public Guid ExternalId { get; }
}