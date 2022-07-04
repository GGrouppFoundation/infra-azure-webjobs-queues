using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class QueueItemHandlerLoggerExtensions
{
    internal static Unit LogSuccess(this ILogger logger, string messageId)
    {
        logger.LogInformation("Queue message '{id}' was proccessed successfully", messageId);
        return default;
    }
}