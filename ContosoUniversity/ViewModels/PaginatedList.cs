namespace ContosoUniversity.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public class PaginatedList<TEntity, TModel> : List<TModel>
    {
        private PaginatedList(
            List<TEntity> items,
            int count,
            int pageIndex,
            int pageSize,
            Func<TEntity, TModel> mapper)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);

            AddRange(items.Select(x => mapper(x)));
        }

        public int PageIndex { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<TEntity, TModel>> CreateAsync(
            IQueryable<TEntity> source,
            int pageIndex,
            int pageSize,
            Func<TEntity, TModel> mapper)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<TEntity, TModel>(items, count, pageIndex, pageSize, mapper);
        }
    }
}