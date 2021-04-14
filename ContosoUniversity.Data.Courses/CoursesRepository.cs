namespace ContosoUniversity.Data.Courses
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Course;

    using Microsoft.EntityFrameworkCore;

    public sealed class CoursesRepository : EfRepository<Course, Models.Course>, ICoursesRepository
    {
        public CoursesRepository(CoursesContext dbContext) : base(dbContext) { }

        public Task<Course[]> GetByDepartmentId(Guid departmentId)
        {
            return DbSet
                .AsNoTracking()
                .Where(x => x.DepartmentExternalId == departmentId)
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();
        }

        public async Task Remove(Guid[] entityIds)
        {
            var courses = await DbSet
                .Where(x => entityIds.Contains(x.ExternalId))
                .ToArrayAsync();

            var notFoundIds = entityIds
                .Except(courses.Select(c => c.ExternalId))
                .ToArray();
            if (notFoundIds.Any())
                throw new AggregateException(
                    notFoundIds
                        .Select(x => new EntityNotFoundException(nameof(courses), x)));

            DbSet.RemoveRange(courses);

            await DbContext.SaveChangesAsync();
        }

        public Task<Course[]> GetByIds(Guid[] entityIds)
        {
            return DbSet
                .AsNoTracking()
                .Where(x => entityIds.Contains(x.ExternalId))
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();
        }

        public Task<bool> ExistsCourseCode(int courseCode)
        {
            return DbSet
                .AsNoTracking()
                .AnyAsync(x => x.CourseCode == courseCode);
        }

        public Task<int> UpdateCourseCredits(int multiplier)
        {
            return DbContext.Database
                .ExecuteSqlInterpolatedAsync(
                    $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}");
        }

        protected override Course ToDomainEntity(Models.Course dataModel)
        {
            return new(
                dataModel.CourseCode,
                dataModel.Title,
                dataModel.Credits,
                dataModel.DepartmentExternalId,
                dataModel.ExternalId);
        }

        protected override void MapDomainEntityOntoDataEntity(Models.Course model, Course entity)
        {
            model.CourseCode = entity.Code;
            model.Title = entity.Title;
            model.Credits = entity.Credits;
            model.DepartmentExternalId = entity.DepartmentId;
            model.ExternalId = entity.EntityId;
        }
    }
}