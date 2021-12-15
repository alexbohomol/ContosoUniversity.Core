namespace ContosoUniversity.Data.Students;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Paging;
using Domain.Student;

using Microsoft.EntityFrameworkCore;

public sealed class StudentsRepository : EfRepository<Student>, IStudentsRepository
{
    public StudentsRepository(StudentsContext dbContext)
        : base(
            dbContext,
            new[] { "Enrollments" })
    {
    }

    public async Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken = default)
    {
        var groups = new List<EnrollmentDateGroup>();

        DbConnection conn = DbContext.Database.GetDbConnection();
        try
        {
            await conn.OpenAsync(cancellationToken);
            await using DbCommand command = conn.CreateCommand();
            command.CommandText =
                @"SELECT EnrollmentDate, COUNT(*) AS StudentCount
                      FROM [std].Student
                      GROUP BY EnrollmentDate";
            DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

            if (reader.HasRows)
                while (await reader.ReadAsync(cancellationToken))
                    groups.Add(
                        new EnrollmentDateGroup(
                            reader.GetDateTime(0),
                            reader.GetInt32(1)));

            await reader.DisposeAsync();
        }
        finally
        {
            await conn.CloseAsync();
        }

        return groups.ToArray();
    }

    public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .Where(x => x.Enrollments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<PagedResult<Student>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        /*
         * TODO: here we don't need default includes in DbQuery
         */
        IQueryable<Student> searchQuery = DbQuery;
        searchQuery = ApplySearch(searchQuery, searchRequest);
        searchQuery = ApplyOrder(searchQuery, orderRequest);

        (Student[] students, PageInfo pageInfo) = await searchQuery
            .AsNoTracking()
            .ToPageAsync(pageRequest, cancellationToken);

        /*
         * TODO: here we don't need default aggregated .ToDomainEntity()
         */
        return new PagedResult<Student>(
            students,
            pageInfo);
    }

    private IQueryable<Student> ApplySearch(IQueryable<Student> source, SearchRequest request)
    {
        return string.IsNullOrEmpty(request.SearchString)
            ? source
            : source.Where(
                s => s.LastName.Contains(request.SearchString)
                     || s.FirstName.Contains(request.SearchString));
    }

    private IQueryable<Student> ApplyOrder(IQueryable<Student> source, OrderRequest request)
    {
        return request.SortOrder switch
        {
            "name_desc" => source.OrderByDescending(s => s.LastName),
            "Date" => source.OrderBy(s => s.EnrollmentDate),
            "date_desc" => source.OrderByDescending(s => s.EnrollmentDate),
            _ => source.OrderBy(s => s.LastName)
        };
    }
}