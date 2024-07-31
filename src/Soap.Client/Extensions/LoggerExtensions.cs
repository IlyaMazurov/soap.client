using Microsoft.Extensions.Logging;

namespace Soap.Client.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(0, LogLevel.Information, "Description={description}, Request={request}")]
    public static partial void LogRequest(this ILogger logger, string description, string request);

    [LoggerMessage(1, LogLevel.Information, "Description={description}, Response={response}")]
    public static partial void LogResponse(this ILogger logger, string description, string response);

    [LoggerMessage(3, LogLevel.Error, "Description={description}, ErrorMessage={errorMessage}")]
    public static partial void LogErrorMessage(this ILogger logger, string description, string errorMessage);
}
