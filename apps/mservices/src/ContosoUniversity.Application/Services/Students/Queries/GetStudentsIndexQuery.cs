namespace ContosoUniversity.Application.Services.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Students.Core;
using global::Students.Core.Projections;

using MediatR;

using SharedKernel.Paging;

public class GetStudentsIndexQuery : IRequest<GetStudentsIndexQueryResult>
{
    public string SortOrder { get; set; }
    public string CurrentFilter { get; set; }
    public string SearchString { get; set; }
    public int? PageNumber { get; set; }
}

public record GetStudentsIndexQueryResult(
    PageInfo PageInfo,
    Student[] Students);

internal class GetStudentsIndexQueryHandler(IStudentsRoRepository studentsRepository)
    : IRequestHandler<GetStudentsIndexQuery, GetStudentsIndexQueryResult>
{
    public async Task<GetStudentsIndexQueryResult> Handle(
        GetStudentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        (Student[] students, PageInfo pageInfo) = await studentsRepository.Search(
            new SearchRequest(request.SearchString),
            new OrderRequest(request.SortOrder),
            new PageRequest(request.PageNumber ?? 1, 3),
            cancellationToken);

        return new GetStudentsIndexQueryResult(pageInfo, students);
    }
}
