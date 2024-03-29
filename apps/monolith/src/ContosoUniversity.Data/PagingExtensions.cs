namespace ContosoUniversity.Data;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly.Paging;

using Microsoft.EntityFrameworkCore;

public static class PagingExtensions
{
    public static async Task<PagedResult<T>> ToPageAsync<T>(
        this IQueryable<T> source,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        return new PagedResult<T>(
            await source.TakePage(request).ToArrayAsync(cancellationToken),
            new PageInfo(
                request,
                await source.CountAsync(cancellationToken)));
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
