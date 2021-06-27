namespace ContosoUniversity.Data.Students
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Paging;
    using Domain.Student;

    using Microsoft.EntityFrameworkCore;

    public sealed class StudentsRepository : EfRepository<Student, Models.Student>, IStudentsRepository
    {
        public StudentsRepository(StudentsContext dbContext) 
            : base(
                dbContext,
                defaultIncludes: new [] { nameof(StudentsContext.Enrollments) })
        { }

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

        public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds)
        {
            var students = await DbQuery
                .AsNoTracking()
                .Where(x => x.Enrollments.Select(e => e.CourseExternalId).Any(id => courseIds.Contains(id)))
                .ToArrayAsync();
            
            return students.Select(ToDomainEntity).ToArray();
        }

        public async Task<PagedResult<Student>> Search(
            SearchRequest searchRequest, 
            OrderRequest orderRequest, 
            PageRequest pageRequest)
        {
            /*
             * TODO: here we don't need default includes in DbQuery
             */
            IQueryable<Models.Student> searchQuery = DbQuery;
            searchQuery = ApplySearch(searchQuery, searchRequest);
            searchQuery = ApplyOrder(searchQuery, orderRequest);
            
            (Models.Student[] students, PageInfo pageInfo) = await searchQuery
                .AsNoTracking()
                .ToPageAsync(pageRequest);
            
            /*
             * TODO: here we don't need default aggregated .ToDomainEntity()
             */
            return new PagedResult<Student>(
                students.Select(ToDomainEntity).ToArray(),
                pageInfo);
        }

        private IQueryable<Models.Student> ApplySearch(IQueryable<Models.Student> source, SearchRequest request)
        {
            return string.IsNullOrEmpty(request.SearchString)
                ? source
                : source.Where(
                    s => s.LastName.Contains(request.SearchString)
                         || s.FirstMidName.Contains(request.SearchString));
        }

        private IQueryable<Models.Student> ApplyOrder(IQueryable<Models.Student> source, OrderRequest request)
        {
            return request.SortOrder switch
            {
                "name_desc" => source.OrderByDescending(s => s.LastName),
                "Date" => source.OrderBy(s => s.EnrollmentDate),
                "date_desc" => source.OrderByDescending(s => s.EnrollmentDate),
                _ => source.OrderBy(s => s.LastName)
            };
        }

        protected override Student ToDomainEntity(Models.Student data)
        {
            return new(
                data.LastName,
                data.FirstMidName,
                data.EnrollmentDate,
                EnrollmentsCollection.From(data.Enrollments.Select(x => x.ToDomainEntity())), 
                data.ExternalId);
        }
        
        protected override void MapDomainEntityOntoDataEntity(Student entity, Models.Student model)
        {
            model.LastName = entity.LastName;
            model.FirstMidName = entity.FirstName;
            model.EnrollmentDate = entity.EnrollmentDate;
            model.ExternalId = entity.EntityId;

            Guid[] current = model.Enrollments.Select(x => x.CourseExternalId).ToArray();
            Guid[] domain = entity.Enrollments.CourseIds.ToArray();

            /*
             * TODO: next feature requirements
             * - what if grade was changed for the existing course?
             */
            if (current.SequenceEqual(domain)) return;

            var toBeDeleted = current.Except(domain).ToArray();
            if (toBeDeleted.Any())
            {
                foreach (Guid courseId in toBeDeleted)
                {
                    var enrollment = model.Enrollments.Single(x => x.CourseExternalId == courseId);
                    model.Enrollments.Remove(enrollment);
                }
            }

            /*
             * TODO: next feature requirements
             * - enrolling students for courses
             */
            var toBeAdded = domain.Except(current).ToArray();
            if (toBeAdded.Any())
            {
                foreach (Guid courseId in toBeAdded)
                {
                    model.Enrollments.Add(new Models.Enrollment
                    {
                        CourseExternalId = courseId,
                        Grade = entity.Enrollments[courseId].Grade.ToDataModel()
                    });
                }
            }
        }
    }
}