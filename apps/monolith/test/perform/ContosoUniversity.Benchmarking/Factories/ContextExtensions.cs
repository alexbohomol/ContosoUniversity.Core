namespace ContosoUniversity.Benchmarking.Factories;

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

internal static class ContextExtensions
{
    public static async Task Populate<TEntity>(this DbContext context,
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
            printBatchStatus(i);
        }
    }
}
