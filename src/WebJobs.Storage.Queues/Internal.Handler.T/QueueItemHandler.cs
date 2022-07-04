using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class QueueMessageHandler<TIn, TOut> : IQueueMessageHandler
    where TIn : notnull
{
    public static QueueMessageHandler<TIn, TOut> Create(
        IQueueItemHandler<TIn, TOut> queueItemHandler,
        ILoggerFactory loggerFactory,
        bool isOutSerialized)
    {
        _ = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return new(queueItemHandler, loggerFactory, isOutSerialized);
    }

    private static readonly JsonSerializerOptions jsonSerializerOptions;

    static QueueMessageHandler()
        =>
        jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    private readonly IQueueItemHandler<TIn, TOut> queueItemHandler;

    private readonly ILogger logger;

    private readonly bool isOutSerialized;

    private QueueMessageHandler(IQueueItemHandler<TIn, TOut> queueItemHandler, ILoggerFactory loggerFactory, bool isOutSerialized)
    {
        this.queueItemHandler = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        var factory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        this.isOutSerialized = isOutSerialized;

        logger = factory.CreateLogger<QueueMessageHandler<TIn, TOut>>();
    }
}