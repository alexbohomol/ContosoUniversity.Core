namespace ContosoUniversity.Data.Courses.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;
using Application.Exceptions;

using Domain.Course;

using Extensions;

using Microsoft.EntityFrameworkCore;

internal class ReadWriteRepository : EfRwRepository<Course>, ICoursesRwRepository
{
    public ReadWriteRepository(ReadWriteContext dbContext) : base(dbContext)
    {
    }

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