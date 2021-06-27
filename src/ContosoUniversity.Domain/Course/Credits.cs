namespace ContosoUniversity.Domain.Course
{
    using System;

    public readonly struct Credits
    {
        public const int MinValue = 0;
        public const int MaxValue = 5;

        private readonly int _credits;

        private Credits(int credits)
        {
            if (credits is < MinValue or > MaxValue)
                throw new ArgumentOutOfRangeException(
                    nameof(credits),
                    $"Provided value: {credits}.");

            _credits = credits;
        }

        public static implicit operator Credits(int credits)
        {
            return new(credits);
        }

        public static implicit operator int(Credits credits)
        {
            return credits._credits;
        }
    }
}