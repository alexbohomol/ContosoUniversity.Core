namespace ContosoUniversity.Data.Courses.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Extensions;

using global::Courses.Core;
using global::Courses.Core.Domain;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Exceptions;

internal class ReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Course>(dbContext), ICoursesRwRepository
{
    public Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default)
    {
        return DbContext.Database
            .ExecuteSqlInterpolatedAsync(
                $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}", cancellationToken);
    }

    public async Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default)
    {
        Course[] courses = await DbQuery
            .Where(x => entityIds.Contains(x.ExternalId))
            .ToArrayAsync(cancellationToken);

        courses.Select(x => x.ExternalId).EnsureCollectionsEqual(
            entityIds,
            id => new EntityNotFoundException(nameof(courses), id));

        DbSet.RemoveRange(courses);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
