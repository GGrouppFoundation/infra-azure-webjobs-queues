using System;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed class BusItemHandlerCanceledException : TaskCanceledException
{
    public BusItemHandlerCanceledException(string messageId, Exception? innerException = null)
        : base($"The service bus queue message '{messageId}' processing was canceled", innerException)
    {
    }
}