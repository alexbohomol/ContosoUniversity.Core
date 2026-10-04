namespace ContosoUniversity.Benchmarking.Factories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

internal static class ContextExtensions
{
    extension(DbContext context)
    {
        public async Task<TEntity[]> PopulateAsync<TEntity>(
            int batchCount,
            int batchSize,
            Func<int, TEntity> entityFactory,
            Action<int> printBatchStatus) where TEntity : class
        {
            var entities = new List<TEntity>();
            foreach (int i in Enumerable.Range(1, batchCount))
            {
                var batch = Enumerable.Range(1, batchSize).Select(entityFactory).ToArray();
                await context.Set<TEntity>().AddRangeAsync(batch);
                await context.SaveChangesAsync();
                entities.AddRange(batch);
                context.ChangeTracker.Clear();
                printBatchStatus(i);
            }
            return [.. entities];
        }

        public async Task<int> CleanupAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
    }
}
