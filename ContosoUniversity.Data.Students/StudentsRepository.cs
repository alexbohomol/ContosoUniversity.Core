namespace ContosoUniversity.Data.Students
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
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

        public async Task Save(Student entity)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.ExternalId == entity.EntityId);
            if (student == null)
            {
                student = new Models.Student();

                UpdateDataModelWithDomain(student, entity);

                await _context.AddAsync(student);
            }
            else
            {
                UpdateDataModelWithDomain(student, entity);
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
            var student = await _context.Students.FirstOrDefaultAsync(x => x.ExternalId == entityId);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), entityId);

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
        }

        public async Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups()
        {
            var groups = new List<EnrollmentDateGroup>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                await using var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT EnrollmentDate, COUNT(*) AS StudentCount
                      FROM [std].Student
                      GROUP BY EnrollmentDate";
                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        groups.Add(
                            new EnrollmentDateGroup(
                                reader.GetDateTime(0),
                                reader.GetInt32(1)));
                    }
                }

                await reader.DisposeAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }

            return groups.ToArray();
        }

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

        private void UpdateDataModelWithDomain(Models.Student model, Student entity)
        {
            model.LastName = entity.LastName;
            model.FirstMidName = entity.FirstName;
            model.EnrollmentDate = entity.EnrollmentDate;
            model.ExternalId = entity.EntityId;
        }
    }
}