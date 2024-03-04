namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.Collections.Generic;

using Application.Contracts.Repositories.ReadOnly.Projections;

using Microsoft.AspNetCore.Mvc.Rendering;

public class EditDepartmentForm : EditDepartmentRequest
{
    public EditDepartmentForm(EditDepartmentRequest request, Dictionary<Guid, string> instructorNames)
    {
        Name = request.Name;
        Budget = request.Budget;
        StartDate = request.StartDate;
        AdministratorId = request.AdministratorId;
        ExternalId = request.ExternalId;
        RowVersion = request.RowVersion;
        InstructorsDropDown = instructorNames.ToSelectList(request.AdministratorId.GetValueOrDefault());
    }

    public EditDepartmentForm()
    {
    }

    public EditDepartmentForm(Department department, Dictionary<Guid, string> instructorsReference)
    {
        ArgumentNullException.ThrowIfNull(department, nameof(department));
        ArgumentNullException.ThrowIfNull(instructorsReference, nameof(instructorsReference));

        Name = department.Name;
        Budget = department.Budget;
        StartDate = department.StartDate;
        AdministratorId = department.AdministratorId;
        ExternalId = department.ExternalId;
        // RowVersion = department.RowVersion,
        InstructorsDropDown = instructorsReference.ToSelectList(department.AdministratorId ?? default);
    }

    public SelectList InstructorsDropDown { get; }
}
