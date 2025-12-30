namespace IntegrationTesting.SharedKernel;

public static class FileAccessExtensions
{
    public static string ToAbsolutePath(this string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                relativePath));
}
