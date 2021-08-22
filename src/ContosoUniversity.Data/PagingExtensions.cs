namespace ContosoUniversity.Data
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts.Paging;

    using Microsoft.EntityFrameworkCore;

    public static class PagingExtensions
    {
        public static async Task<PagedResult<T>> ToPageAsync<T>(
            this IQueryable<T> source, 
            PageRequest request,
            CancellationToken cancellationToken = default) 
            => new(
                await source.TakePage(request).ToArrayAsync(cancellationToken), 
                new PageInfo(
                    request,
                    await source.CountAsync(cancellationToken)));

        private static IQueryable<T> TakePage<T>(
            this IQueryable<T> source,
            PageRequest request) 
            => source
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
    }
}