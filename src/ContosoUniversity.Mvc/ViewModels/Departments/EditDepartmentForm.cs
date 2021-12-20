namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.Collections.Generic;

using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Departments.Commands;

using Microsoft.AspNetCore.Mvc.Rendering;

public class EditDepartmentForm : EditDepartmentCommand
{
    public EditDepartmentForm(EditDepartmentCommand command, Dictionary<Guid, string> instructorNames)
    {
        Name = command.Name;
        Budget = command.Budget;
        StartDate = command.StartDate;
        AdministratorId = command.AdministratorId;
        ExternalId = command.ExternalId;
        RowVersion = command.RowVersion;
        InstructorsDropDown = instructorNames.ToSelectList(command.AdministratorId.GetValueOrDefault());
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