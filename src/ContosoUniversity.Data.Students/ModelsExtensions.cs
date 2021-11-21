namespace ContosoUniversity.Data.Students;

using System;

using Domain.Student;

internal static class ModelsExtensions
{
    public static Enrollment ToDomainEntity(this Models.Enrollment data)
    {
        return new Enrollment(
            data.CourseExternalId,
            data.Grade.ToDomainEntity());
    }

    private static Grade ToDomainEntity(this Models.Grade? grade)
    {
        return grade switch
        {
            Models.Grade.A => Grade.A,
            Models.Grade.B => Grade.B,
            Models.Grade.C => Grade.C,
            Models.Grade.D => Grade.D,
            Models.Grade.F => Grade.F,
            null => Grade.Undefined,
            _ => throw new ArgumentOutOfRangeException(nameof(grade), grade, null)
        };
    }

    public static Models.Grade? ToDataModel(this Grade grade)
    {
        return grade switch
        {
            Grade.A => Models.Grade.A,
            Grade.B => Models.Grade.B,
            Grade.C => Models.Grade.C,
            Grade.D => Models.Grade.D,
            Grade.F => Models.Grade.F,
            Grade.Undefined => null,
            _ => throw new ArgumentOutOfRangeException(nameof(grade), grade, null)
        };
    }
}