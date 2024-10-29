namespace ContosoUniversity.Domain;

public interface IIdentifiable<T> where T : struct
{
    T ExternalId { get; }
}
