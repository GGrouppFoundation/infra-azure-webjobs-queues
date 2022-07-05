using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class QueueItemHandlerLoggerExtensions
{
    internal static Unit LogFailure(this ILogger logger, string messageId, string errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            logger.LogError("Queue message '{id}' was unsuccessfully proccessed and will be deleted from the queue", messageId);
            return default;
        }

        logger.LogError("Queue message '{id}' was unsuccessfully proccessed and will be deleted from the queue: {message}", messageId, errorMessage);
        return default;
    }
}