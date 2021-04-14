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

        public async Task<Course[]> GetByDepartmentId(Guid departmentId)
        {
            var courses = await DbQuery
                .AsNoTracking()
                .Where(x => x.DepartmentExternalId == departmentId)
                .ToArrayAsync();
            
            return courses.Select(ToDomainEntity).ToArray();
        }

        public async Task Remove(Guid[] entityIds)
        {
            var courses = await DbQuery
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

        public async Task<Course[]> GetByIds(Guid[] entityIds)
        {
            var courses = await DbQuery
                .AsNoTracking()
                .Where(x => entityIds.Contains(x.ExternalId))
                .ToArrayAsync();
            
            return courses.Select(ToDomainEntity).ToArray();
        }

        public Task<bool> ExistsCourseCode(int courseCode)
        {
            return DbQuery
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

        protected override void MapDomainEntityOntoDataEntity(Course entity, Models.Course model)
        {
            model.CourseCode = entity.Code;
            model.Title = entity.Title;
            model.Credits = entity.Credits;
            model.DepartmentExternalId = entity.DepartmentId;
            model.ExternalId = entity.EntityId;
        }
    }
}