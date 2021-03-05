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

        public Task<Course[]> GetAll() =>
            _context.Courses
                .AsNoTracking()
                .Select(x => ToDomainEntity(x))
                .ToArrayAsync();

        private static Course ToDomainEntity(Models.Course course) => new(
            course.CourseCode,
            course.Title,
            course.Credits,
            course.DepartmentExternalId,
            course.ExternalId);

        public async Task Save(Course entity)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.ExternalId == entity.EntityId);

            if (course == null)
            {
                await _context.AddAsync(new Models.Course
                {
                    CourseCode = entity.Code,
                    Title = entity.Title,
                    Credits = entity.Credits,
                    DepartmentExternalId = entity.DepartmentId,
                    ExternalId = entity.EntityId
                });
            }
            else
            {
                course.Credits = entity.Credits;
                course.DepartmentExternalId = entity.DepartmentId;
                course.Title = entity.Title;
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

        public Task Remove(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateCourseCredits(int multiplier) =>
            _context.Database
                .ExecuteSqlInterpolatedAsync(
                    $"UPDATE [crs].[Course] SET Credits = Credits * {multiplier}");
    }
}