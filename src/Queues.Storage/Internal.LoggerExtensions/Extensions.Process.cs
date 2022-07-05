using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class QueueItemHandlerLoggerExtensions
{
    internal static Unit ProcessFailure(this ILogger logger, QueueItemFailure failure, string messageId)
    {
        if (failure.ReturnToQueue)
        {
            throw new QueueItemProcessorOperationException(queueMessageId: messageId, failureMessage: failure.FailureMessage);
        }

        return logger.LogFailure(messageId: messageId, errorMessage: failure.FailureMessage);
    }
}