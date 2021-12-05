namespace ContosoUniversity.Data.Courses;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Course;

using Extensions;

using Microsoft.EntityFrameworkCore;

public class CoursesRoRepository : EfRoRepository<Course>, ICoursesRoRepository
{
    public CoursesRoRepository(CoursesContext dbContext) : base(dbContext)
    {
    }

    public async Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default)
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