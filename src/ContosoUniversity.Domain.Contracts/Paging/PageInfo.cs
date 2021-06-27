namespace ContosoUniversity.Domain.Contracts.Paging
{
    using System;

    public class PageInfo
    {
        public PageInfo(PageRequest request, int count)
        {
            PageIndex = request.PageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) request.PageSize);
        }
        
        public int PageIndex { get; }
        private int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}