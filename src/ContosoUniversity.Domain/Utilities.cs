namespace ContosoUniversity.Domain
{
    using System;
    using System.Collections.Generic;

    public static class Utilities
    {
        public static string ToDisplayString(this IEnumerable<Guid> guids) => string.Join(", ", guids);
    }
}