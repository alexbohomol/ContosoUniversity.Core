namespace ContosoUniversity.Mvc.Controllers;

using Microsoft.Extensions.Logging;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        level: LogLevel.Information,
        message: "User visited page {Action} at {Controller}")]
    public static partial void LogPageVisited(this ILogger logger, string action, string controller);
}
