namespace ContosoUniversity.Data.Courses
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain;
    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Microsoft.EntityFrameworkCore;

    public class CoursesRepository : ICoursesRepository
    {
        private readonly CoursesContext _context;

        public CoursesRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<Course> GetById(Guid entityId)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == entityId);

            return course == null
                ? null
                : ToDomainEntity(course);
        }

        public Task<Course[]> GetAll()
        {
            return _context.Courses
                .AsNoTracking()
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();
        }

        public Task<Course[]> GetByDepartmentId(Guid departmentId)
        {
            return _context.Courses
                .AsNoTracking()
                .Where(x => x.DepartmentExternalId == departmentId)
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();
        }

        public async Task Save(Course entity)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.ExternalId == entity.EntityId);
            if (course == null)
            {
                course = new Models.Course();

                UpdateDataModelWithDomain(course, entity);

                await _context.AddAsync(course);
            }
            else
            {
                UpdateDataModelWithDomain(course, entity);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //Log the error (uncomment ex variable name and write a log.)
                throw new PersistenceException(
                    "Unable to save changes. Try again, and if the problem persists, see your system administrator.",
                    exception);
            }
        }

        public async Task Remove(Guid entityId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.ExternalId == entityId);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), entityId);

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid[] entityIds)
        {
            var courses = await _context.Courses
                .Where(x => entityIds.Contains(x.ExternalId))
                .ToArrayAsync();

            var notFoundIds = entityIds
                .Except(courses.Select(c => c.ExternalId))
                .ToArray();
            if (notFoundIds.Any())
                throw new AggregateException(
                    notFoundIds
                        .Select(x => new EntityNotFoundException(nameof(courses), x)));

            _context.Courses.RemoveRange(courses);

            await _context.SaveChangesAsync();
        }

        public Task<Course[]> GetByIds(Guid[] entityIds)
        {
            return _context.Courses
                .AsNoTracking()
                .Where(x => entityIds.Contains(x.ExternalId))
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();
        }

        public Task<bool> ExistsCourseCode(int courseCode)
        {
            return _context.Courses
                .AsNoTracking()
                .AnyAsync(x => x.CourseCode == courseCode);
        }

        public Task<int> UpdateCourseCredits(int multiplier)
        {
            return _context.Database
                .ExecuteSqlInterpolatedAsync(
                    $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}");
        }

        private static Course ToDomainEntity(Models.Course dataModel)
        {
            return new(
                dataModel.CourseCode,
                dataModel.Title,
                dataModel.Credits,
                dataModel.DepartmentExternalId,
                dataModel.ExternalId);
        }

        private void UpdateDataModelWithDomain(Models.Course model, Course entity)
        {
            model.CourseCode = entity.Code;
            model.Title = entity.Title;
            model.Credits = entity.Credits;
            model.DepartmentExternalId = entity.DepartmentId;
            model.ExternalId = entity.EntityId;
        }
    }
}