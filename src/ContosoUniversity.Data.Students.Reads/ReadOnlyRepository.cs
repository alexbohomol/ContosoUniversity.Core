namespace ContosoUniversity.Data.Students.Reads;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application;
using Application.Paging;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

internal sealed class ReadOnlyRepository : EfRoRepository<StudentReadModel>, IStudentsRoRepository
{
    public ReadOnlyRepository(ReadOnlyContext dbContext) : base(dbContext)
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

    public async Task<StudentReadModel[]> GetStudentsEnrolledForCourses(Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .Where(x => x.Enrollments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<PagedResult<StudentReadModel>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        /*
         * TODO: here we don't need default includes in DbQuery
         */
        IQueryable<StudentReadModel> searchQuery = DbQuery;
        searchQuery = ApplySearch(searchQuery, searchRequest);
        searchQuery = ApplyOrder(searchQuery, orderRequest);

        (StudentReadModel[] students, PageInfo pageInfo) = await searchQuery
            .AsNoTracking()
            .ToPageAsync(pageRequest, cancellationToken);

        return new PagedResult<StudentReadModel>(
            students,
            pageInfo);
    }

    private IQueryable<StudentReadModel> ApplySearch(IQueryable<StudentReadModel> source, SearchRequest request)
    {
        return string.IsNullOrEmpty(request.SearchString)
            ? source
            : source.Where(
                s => s.LastName.Contains(request.SearchString)
                     || s.FirstName.Contains(request.SearchString));
    }

    private IQueryable<StudentReadModel> ApplyOrder(IQueryable<StudentReadModel> source, OrderRequest request)
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