namespace Students.Core.Paging;

using System;

public class PageInfo(PageRequest request, int count)
{
    public int PageIndex => request.PageNumber;
    private int TotalPages => (int)Math.Ceiling(count / (double)request.PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
