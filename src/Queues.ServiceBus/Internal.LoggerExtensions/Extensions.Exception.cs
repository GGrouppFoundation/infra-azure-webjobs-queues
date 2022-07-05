using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class BusHandlerLoggerExtensions
{
    internal static Unit LogException(this ILogger logger, string messageId, Exception exception)
    {
        logger.LogError(exception, "Service bus queue message '{id}' was unsuccessfully proccessed and will be deleted from the queue", messageId);
        return default;
    }
}