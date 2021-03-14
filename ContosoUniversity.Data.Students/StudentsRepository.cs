namespace ContosoUniversity.Data.Students
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Student;

    using Microsoft.EntityFrameworkCore;

    public class StudentsRepository : IStudentsRepository
    {
        private readonly StudentsContext _context;

        public StudentsRepository(StudentsContext context)
        {
            _context = context;
        }
        
        public async Task<Student> GetById(Guid entityId)
        {
            var entity = await _context.Students
                .Include(s => s.Enrollments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == entityId);

            return entity == null
                ? null
                : ToDomainEntity(entity);
        }

        public Task<Student[]> GetAll() => throw new NotImplementedException();

        public Task Save(Student entity) => throw new NotImplementedException();

        public Task Remove(Guid entityId) => throw new NotImplementedException();

        private Student ToDomainEntity(Models.Student data)
        {
            return new(
                data.LastName,
                data.FirstMidName,
                data.EnrollmentDate,
                data.Enrollments.Select(ToDomainEntity).ToArray(),
                data.ExternalId);
        }

        private Enrollment ToDomainEntity(Models.Enrollment data)
        {
            return new(
                data.CourseExternalId,
                ToDomainEntity(data.Grade));
        }

        private Grade ToDomainEntity(Models.Grade? data)
        {
            return data switch
            {
                Models.Grade.A => Grade.A,
                Models.Grade.B => Grade.B,
                Models.Grade.C => Grade.C,
                Models.Grade.D => Grade.D,
                Models.Grade.F => Grade.F,
                null => Grade.Undefined,
                _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };
        }
    }
}