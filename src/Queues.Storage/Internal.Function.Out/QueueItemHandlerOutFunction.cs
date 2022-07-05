using System;

namespace GGroupp.Infra;

internal sealed partial class QueueItemHandlerOutFunction
{
    private readonly IQueueMessageHandler queueMessageHandler;

    public QueueItemHandlerOutFunction(IQueueMessageHandler queueMessageHandler)
        =>
        this.queueMessageHandler = queueMessageHandler ?? throw new ArgumentNullException(nameof(queueMessageHandler));
}