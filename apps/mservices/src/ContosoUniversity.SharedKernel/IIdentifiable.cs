namespace ContosoUniversity.SharedKernel;

public interface IIdentifiable<T> where T : struct
{
    T ExternalId { get; }
}
