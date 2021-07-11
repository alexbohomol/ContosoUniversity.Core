namespace ContosoUniversity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Departments;

    using Microsoft.EntityFrameworkCore;

    public static class ContextsExtensions
    {
        public static Task<Dictionary<Guid, string>> GetDepartmentsNames(this DepartmentsContext context)
        {
            return context
                .Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(
                    x => x.ExternalId,
                    x => x.Name);
        }
        
        public static Task<Dictionary<int, string>> GetInstructorsNames(this DepartmentsContext context)
        {
            return context
                   .Instructors
                   .AsNoTracking()
                   .ToDictionaryAsync(
                       x => x.Id,
                       x => x.FullName);
        }
    }
}