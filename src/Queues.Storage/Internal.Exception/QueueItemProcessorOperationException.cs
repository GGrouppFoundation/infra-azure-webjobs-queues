using System;
using System.Text;

namespace GGroupp.Infra;

internal sealed class QueueItemProcessorOperationException : InvalidOperationException
{
    public QueueItemProcessorOperationException(string queueMessageId, string? failureMessage)
        : base(BuildMessage(queueMessageId: queueMessageId, failureMessage: failureMessage))
    {
    }

    public QueueItemProcessorOperationException(string queueMessageId, Exception innerException)
        : base(BuildMessage(queueMessageId), innerException)
    {
    }

    private static string BuildMessage(string queueMessageId, string? failureMessage = null)
    {
        var errorBuilder = new StringBuilder($"The queue message '{queueMessageId}' was unsuccessfully proccessed with an unexpected error");

        if (string.IsNullOrEmpty(failureMessage))
        {
            errorBuilder.ToString();
        }
        
        return errorBuilder.Append($": {failureMessage}").ToString();
    }
}