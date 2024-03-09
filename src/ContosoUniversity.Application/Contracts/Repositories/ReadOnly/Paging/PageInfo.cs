namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Paging;

using System;

public class PageInfo(PageRequest request, int count)
{
    public int PageIndex { get; } = request.PageNumber;
    private int TotalPages { get; } = (int)Math.Ceiling(count / (double)request.PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
