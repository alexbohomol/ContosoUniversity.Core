namespace ContosoUniversity.Benchmarking.Factories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SharedKernel;

internal static class ContextExtensions
{
    extension(DbContext context)
    {
        public async Task<Guid[]> PopulateAsync<TEntity>(
            int batchCount,
            int batchSize,
            Func<int, TEntity> entityFactory,
            Action<int> printBatchStatus) where TEntity : class, IIdentifiable<Guid>
        {
            var ids = new List<Guid>();
            foreach (int i in Enumerable.Range(1, batchCount))
            {
                var entities = Enumerable.Range(1, batchSize).Select(entityFactory).ToArray();
                await context.Set<TEntity>().AddRangeAsync(entities);
                await context.SaveChangesAsync();
                ids.AddRange(entities.Select(x => x.ExternalId));
                context.ChangeTracker.Clear();
                printBatchStatus(i);
            }
            return [.. ids];
        }

        public async Task<int> CleanupAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
    }
}
