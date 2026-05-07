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
        var src = source.Order().ToArray();
        var trg = target.Order().ToArray();
        if (src.SequenceEqual(trg))
        {
            return;
        }

        throw new AggregateException(src
            .Except(trg)
            .Select(exceptionFactory));
    }
}
