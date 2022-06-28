using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class QueueItemHandlerFunction
{
    private readonly IQueueItemHandler queueItemHandler;

    private readonly ILogger logger;

    public QueueItemHandlerFunction(IQueueItemHandler queueItemHandler, ILoggerFactory loggerFactory)
    {
        this.queueItemHandler = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        var factory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        logger = factory.CreateLogger<QueueItemHandlerFunction>();
    }
}