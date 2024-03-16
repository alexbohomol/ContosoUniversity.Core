namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CreateDepartmentForm : CreateDepartmentRequest
{
    public CreateDepartmentForm()
    {
    }

    public CreateDepartmentForm(
        CreateDepartmentRequest request,
        IDictionary<Guid, string> instructorNames)
    {
        Name = request.Name;
        Budget = request.Budget;
        StartDate = request.StartDate;
        AdministratorId = request.AdministratorId;
        InstructorsDropDown = instructorNames.ToSelectList();
    }

    public SelectList InstructorsDropDown { get; set; }
}
