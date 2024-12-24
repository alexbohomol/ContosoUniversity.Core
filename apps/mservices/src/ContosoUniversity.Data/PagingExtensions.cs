namespace ContosoUniversity.Data;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Paging;

public static class PagingExtensions
{
    public static async Task<PagedResult<T>> ToPageAsync<T>(
        this IQueryable<T> source,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        var calculator = new PageInfoCalculator(
            request,
            await source.CountAsync(cancellationToken));

        return new PagedResult<T>(
            await source.TakePage(request).ToArrayAsync(cancellationToken),
            new PageInfo(
                calculator.PageIndex,
                calculator.HasPreviousPage,
                calculator.HasNextPage));
    }

    private static IQueryable<T> TakePage<T>(
        this IQueryable<T> source,
        PageRequest request)
    {
        return source
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

file class PageInfoCalculator(PageRequest request, int count)
{
    public int PageIndex => request.PageNumber;
    private int TotalPages => (int)Math.Ceiling(count / (double)request.PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
