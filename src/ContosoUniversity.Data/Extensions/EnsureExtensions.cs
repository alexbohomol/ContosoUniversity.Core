namespace ContosoUniversity.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

public static class EnsureExtensions
{
    public static void EnsureCollectionsEqual(
        this IEnumerable<Guid> source,
        IEnumerable<Guid> target,
        Func<Guid, Exception> exceptionFactory)
    {
        if (source.Count() == target.Count() && !source.Except(target).Any())
            return;

        throw new AggregateException(source
            .Except(target)
            .Select(exceptionFactory));
    }
}
