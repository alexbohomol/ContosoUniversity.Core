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

        public static implicit operator CourseCode (int code) => new(code);
        public static implicit operator int (CourseCode code) => code._code;
    }
}