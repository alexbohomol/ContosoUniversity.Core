namespace ContosoUniversity.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Departments;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using ViewModels;

    public static class ContextsExtensions
    {
        public static async Task<SelectList> ToDepartmentsDropDownList(
            this DepartmentsContext context,
            Guid selectedDepartment = default)
        {
            var departments = await context
                .Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToArrayAsync();

            return departments.ToSelectList(selectedDepartment);
        }
    }
}