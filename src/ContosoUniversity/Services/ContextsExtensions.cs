namespace ContosoUniversity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Departments;

    using Microsoft.EntityFrameworkCore;

    public static class ContextsExtensions
    {
        public static Task<Dictionary<Guid, string>> GetInstructorsNames(this DepartmentsContext context)
        {
            return context
                   .Instructors
                   .AsNoTracking()
                   .ToDictionaryAsync(
                       x => x.ExternalId,
                       x => x.FullName);
        }
    }
}