namespace ContosoUniversity.Domain
{
    using System;

    public readonly struct CourseCode
    {
        public const int MinValue = 1000;
        public const int MaxValue = 9999;

        private readonly int _code;

        private CourseCode(int code)
        {
            if (code < MinValue || code > MaxValue)
                throw new ArgumentOutOfRangeException(
                    nameof(code),
                    $"Provided value: {code}.");

            _code = code;
        }

        /// <summary>
        ///     https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators
        /// </summary>
        public static implicit operator CourseCode(int code)
        {
            return new(code);
        }

        public static implicit operator int(CourseCode code)
        {
            return code._code;
        }

        public override string ToString() => _code.ToString();
    }
}