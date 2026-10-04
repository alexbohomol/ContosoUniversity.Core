namespace ContosoUniversity.Benchmarking.Factories;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

internal static class ContextExtensions
{
    extension(DbContext context)
    {
        public async Task PopulateAsync<TEntity>(
            int batchCount,
            int batchSize,
            Func<int, TEntity> entityFactory,
            Action<int> printBatchStatus) where TEntity : class
        {
            foreach (int i in Enumerable.Range(1, batchCount))
            {
                var entities = Enumerable.Range(1, batchSize).Select(entityFactory);
                await context.Set<TEntity>().AddRangeAsync(entities);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
                printBatchStatus(i);
            }
        }

        public async Task CleanupAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
    }
}
