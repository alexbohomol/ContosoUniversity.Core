namespace Courses.Core.Domain;

using System;

public readonly struct Credits
{
    public const int MinValue = 0;
    public const int MaxValue = 5;

    private readonly int _credits;

    private Credits(int credits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(credits, MinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(credits, MaxValue);

        _credits = credits;
    }

    public static implicit operator Credits(int credits) => ToCredits(credits);

    public static implicit operator int(Credits credits) => ToInt32(credits);

    public static Credits ToCredits(int credits) => new(credits);

    public static int ToInt32(Credits credits) => credits._credits;
}
