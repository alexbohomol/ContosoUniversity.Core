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

public sealed class CoursesRepository : EfRepository<Course, Models.Course>, ICoursesRepository
{
    public CoursesRepository(CoursesContext dbContext) : base(dbContext)
    {
    }

    public async Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default)
    {
        Models.Course[] courses = await DbQuery
            .AsNoTracking()
            .Where(x => x.DepartmentExternalId == departmentId)
            .ToArrayAsync(cancellationToken);

        return courses.Select(ToDomainEntity).ToArray();
    }

    public async Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default)
    {
        Models.Course[] courses = await DbQuery
            .Where(x => entityIds.Contains(x.ExternalId))
            .ToArrayAsync(cancellationToken);

        courses.Select(x => x.ExternalId).EnsureCollectionsEqual(
            entityIds,
            id => new EntityNotFoundException(nameof(courses), id));

        DbSet.RemoveRange(courses);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Course[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default)
    {
        Models.Course[] courses = await DbQuery
            .AsNoTracking()
            .Where(x => entityIds.Contains(x.ExternalId))
            .ToArrayAsync(cancellationToken);

        courses.Select(x => x.ExternalId).EnsureCollectionsEqual(
            entityIds,
            id => new EntityNotFoundException(nameof(courses), id));

        return courses.Select(ToDomainEntity).ToArray();
    }

    public Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default)
    {
        return DbQuery
            .AsNoTracking()
            .AnyAsync(x => x.CourseCode == courseCode, cancellationToken);
    }

    public Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default)
    {
        return DbContext.Database
            .ExecuteSqlInterpolatedAsync(
                $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}", cancellationToken);
    }

    protected override Course ToDomainEntity(Models.Course dataModel)
    {
        return new Course(
            dataModel.CourseCode,
            dataModel.Title,
            dataModel.Credits,
            dataModel.DepartmentExternalId,
            dataModel.ExternalId);
    }

    protected override void MapDomainEntityOntoDataEntity(Course entity, Models.Course model)
    {
        model.CourseCode = entity.Code;
        model.Title = entity.Title;
        model.Credits = entity.Credits;
        model.DepartmentExternalId = entity.DepartmentId;
        model.ExternalId = entity.ExternalId;
    }
}