using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class BusMessageHandler<TIn, TOut> : IBusMessageHandler
    where TIn : notnull
{
    public static BusMessageHandler<TIn, TOut> Create(
        IQueueItemHandler<TIn, TOut> queueItemHandler,
        ILoggerFactory loggerFactory,
        int maxDeliveryCount,
        bool isOutSerializable)
    {
        _ = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return new(queueItemHandler, loggerFactory, maxDeliveryCount, isOutSerializable);
    }

    private static readonly JsonSerializerOptions jsonSerializerOptions;

    static BusMessageHandler()
        =>
        jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    private readonly IQueueItemHandler<TIn, TOut> queueItemHandler;

    private readonly ILogger logger;

    private readonly bool isOutSerializable;

    private readonly int maxDeliveryCount;

    private BusMessageHandler(IQueueItemHandler<TIn, TOut> queueItemHandler, ILoggerFactory loggerFactory, int maxDeliveryCount, bool isOutSerializable)
    {
        this.queueItemHandler = queueItemHandler;
        logger = loggerFactory.CreateLogger<BusMessageHandler>();
        this.maxDeliveryCount = maxDeliveryCount;
        this.isOutSerializable = isOutSerializable;
    }
}