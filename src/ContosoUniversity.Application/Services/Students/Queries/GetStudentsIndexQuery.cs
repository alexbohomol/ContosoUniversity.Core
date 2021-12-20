namespace ContosoUniversity.Application.Services.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Paging;
using Contracts.Repositories.ReadOnly.Projections;

using MediatR;

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

public class GetStudentsIndexQueryHandler : IRequestHandler<GetStudentsIndexQuery, GetStudentsIndexQueryResult>
{
    private readonly IStudentsRoRepository _studentsRepository;

    public GetStudentsIndexQueryHandler(IStudentsRoRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task<GetStudentsIndexQueryResult> Handle(
        GetStudentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        (Student[] students, PageInfo pageInfo) = await _studentsRepository.Search(
            new SearchRequest(request.SearchString),
            new OrderRequest(request.SortOrder),
            new PageRequest(request.PageNumber ?? 1, 3),
            cancellationToken);

        return new GetStudentsIndexQueryResult(pageInfo, students);
    }
}