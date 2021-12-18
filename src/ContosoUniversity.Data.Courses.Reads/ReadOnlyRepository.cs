namespace ContosoUniversity.Data.Courses.Reads;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

using Extensions;

using Microsoft.EntityFrameworkCore;

internal class ReadOnlyRepository : EfRoRepository<Course>, ICoursesRoRepository
{
    public ReadOnlyRepository(ReadOnlyContext dbContext) : base(dbContext)
    {
    }

    public async Task<Course[]> GetByDepartmentId(Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .Where(x => x.DepartmentId == departmentId)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Course[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default)
    {
        Course[] courses = await DbQuery
            .AsNoTracking()
            .Where(x => entityIds.Contains(x.ExternalId))
            .ToArrayAsync(cancellationToken);

        courses.Select(x => x.ExternalId).EnsureCollectionsEqual(
            entityIds,
            id => new EntityNotFoundException(nameof(courses), id));

        return courses.ToArray();
    }

    public Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default)
    {
        return DbQuery
            .AsNoTracking()
            .AnyAsync(x => x.Code == courseCode, cancellationToken);
    }
}