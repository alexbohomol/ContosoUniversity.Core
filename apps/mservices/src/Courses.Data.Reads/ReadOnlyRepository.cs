namespace Courses.Data.Reads;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Data;

using Core;
using Core.Projections;

using Microsoft.EntityFrameworkCore;

internal class ReadOnlyRepository(ReadOnlyContext dbContext) : EfRoRepository<Course>(dbContext), ICoursesRoRepository
{
    public async Task<Course[]> GetByDepartmentId(Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .Where(x => x.DepartmentId == departmentId)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, string>> GetCourseTitlesReference(
        Guid[] entityIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .Select(x => new
            {
                x.ExternalId,
                x.Title
            })
            .Where(x => entityIds.Contains(x.ExternalId))
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.Title,
                cancellationToken);
    }

    public Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default)
    {
        return DbQuery
            .AsNoTracking()
            .AnyAsync(x => x.Code == courseCode, cancellationToken);
    }
}
