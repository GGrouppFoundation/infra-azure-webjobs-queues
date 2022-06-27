using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class QueueItemProcessorFunction
{
    private readonly IQueueItemProcessor queueItemProcessor;

    private readonly ILogger logger;

    public QueueItemProcessorFunction(IQueueItemProcessor queueItemProcessor, ILoggerFactory loggerFactory)
    {
        this.queueItemProcessor = queueItemProcessor ?? throw new ArgumentNullException(nameof(queueItemProcessor));
        var factory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        logger = factory.CreateLogger<QueueItemProcessorFunction>();
    }
}