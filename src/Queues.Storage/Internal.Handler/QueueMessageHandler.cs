using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class QueueMessageHandler : IQueueMessageHandler
{
    public static QueueMessageHandler Create(IQueueItemHandler queueItemHandler, ILoggerFactory loggerFactory)
    {
        _ = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return new(queueItemHandler, loggerFactory);
    }

    private readonly IQueueItemHandler queueItemHandler;

    private readonly ILogger logger;

    private QueueMessageHandler(IQueueItemHandler queueItemHandler, ILoggerFactory loggerFactory)
    {
        this.queueItemHandler = queueItemHandler;
        logger = loggerFactory.CreateLogger<QueueMessageHandler>();
    }
}