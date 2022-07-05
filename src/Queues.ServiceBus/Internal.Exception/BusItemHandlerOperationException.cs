using System;
using System.Text;

namespace GGroupp.Infra;

internal sealed class BusItemHandlerOperationException : InvalidOperationException
{
    public BusItemHandlerOperationException(string queueMessageId, string? failureMessage)
        : base(BuildMessage(queueMessageId: queueMessageId, failureMessage: failureMessage))
    {
    }

    public BusItemHandlerOperationException(string queueMessageId, Exception innerException)
        : base(BuildMessage(queueMessageId), innerException)
    {
    }

    private static string BuildMessage(string queueMessageId, string? failureMessage = null)
    {
        var errorBuilder = new StringBuilder($"The service bus queue message '{queueMessageId}' was unsuccessfully proccessed with an unexpected error");

        if (string.IsNullOrEmpty(failureMessage))
        {
            errorBuilder.ToString();
        }
        
        return errorBuilder.Append($": {failureMessage}").ToString();
    }
}