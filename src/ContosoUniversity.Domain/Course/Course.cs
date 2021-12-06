namespace ContosoUniversity.Domain.Course;

using System;

public record CourseReadModel(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;

public class Course : IIdentifiable<Guid>
{
    private Guid _departmentId;
    private string _title;

    private Course()
    {
    }

    private Course(
        CourseCode code,
        string title,
        Credits credits,
        Guid departmentId,
        Guid externalId)
    {
        Code = code;
        Title = title;
        Credits = credits;
        DepartmentId = departmentId;
        ExternalId = externalId;
    }

    public CourseCode Code { get; }

    public string Title
    {
        get => _title;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    "Title cannot be null or whitespace.");

            _title = value;
        }
    }

    public Credits Credits { get; private set; }

    public Guid DepartmentId
    {
        get => _departmentId;
        private set
        {
            if (value == default)
                throw new ArgumentException(
                    "DepartmentId value cannot be set to empty.");

            _departmentId = value;
        }
    }

    public Guid ExternalId { get; }

    public static Course Create(CourseCode code, string title, Credits credits, Guid departmentId)
    {
        return new Course(code, title, credits, departmentId, Guid.NewGuid());
    }

    public void Update(
        string title,
        Credits credits,
        Guid departmentId)
    {
        Title = title;
        Credits = credits;
        DepartmentId = departmentId;
    }
}