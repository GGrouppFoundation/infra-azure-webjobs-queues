using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class BusMessageHandler : IBusMessageHandler
{
    public static BusMessageHandler Create(IQueueItemHandler queueItemHandler, ILoggerFactory loggerFactory, int maxDeliveryCount)
    {
        _ = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return new(queueItemHandler, loggerFactory, maxDeliveryCount);
    }

    private readonly IQueueItemHandler queueItemHandler;

    private readonly ILogger logger;

    private readonly int maxDeliveryCount;

    private BusMessageHandler(IQueueItemHandler queueItemHandler, ILoggerFactory loggerFactory, int maxDeliveryCount)
    {
        this.queueItemHandler = queueItemHandler;
        this.maxDeliveryCount = maxDeliveryCount;
        logger = loggerFactory.CreateLogger<BusMessageHandler>();
    }
}