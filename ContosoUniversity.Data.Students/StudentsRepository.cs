namespace ContosoUniversity.Data.Students
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Student;

    using Microsoft.EntityFrameworkCore;

    public class StudentsRepository : EfRepository<Student, Models.Student>, IStudentsRepository
    {
        public StudentsRepository(StudentsContext dbContext) : base(dbContext) { }

        public async Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups()
        {
            var groups = new List<EnrollmentDateGroup>();

            var conn = DbContext.Database.GetDbConnection();
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

        protected override Student ToDomainEntity(Models.Student data)
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
        
        protected override void MapDomainEntityOntoDataEntity(Models.Student model, Student entity)
        {
            model.LastName = entity.LastName;
            model.FirstMidName = entity.FirstName;
            model.EnrollmentDate = entity.EnrollmentDate;
            model.ExternalId = entity.EntityId;
        }
    }
}