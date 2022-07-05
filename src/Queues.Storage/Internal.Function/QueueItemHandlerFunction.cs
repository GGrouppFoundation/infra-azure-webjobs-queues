using System;

namespace GGroupp.Infra;

internal sealed partial class QueueItemHandlerFunction
{
    private readonly IQueueMessageHandler queueMessageHandler;

    public QueueItemHandlerFunction(IQueueMessageHandler queueMessageHandler)
        =>
        this.queueMessageHandler = queueMessageHandler ?? throw new ArgumentNullException(nameof(queueMessageHandler));
}