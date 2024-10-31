namespace Departments.Core.Domain;

using System;

public record OfficeAssignment
{
    public const int TitleMaxLength = 50;

    public OfficeAssignment(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (title.Length > TitleMaxLength)
        {
            throw new ArgumentException($"Office title cannot exceed length of {TitleMaxLength}", nameof(title));
        }

        Title = title;
    }

    public string Title { get; }
}
