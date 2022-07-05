using System;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed class QueueItemProcessorCanceledException : TaskCanceledException
{
    public QueueItemProcessorCanceledException(string messageId, Exception? innerException = null)
        : base($"The queue message '{messageId}' processing was canceled", innerException)
    {
    }
}