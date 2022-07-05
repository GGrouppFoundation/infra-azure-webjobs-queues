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
        bool isOutSerializable)
    {
        _ = queueItemHandler ?? throw new ArgumentNullException(nameof(queueItemHandler));
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return new(queueItemHandler, loggerFactory, isOutSerializable);
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

    private readonly bool isOutSerializable;

    private QueueMessageHandler(IQueueItemHandler<TIn, TOut> queueItemHandler, ILoggerFactory loggerFactory, bool isOutSerializable)
    {
        this.queueItemHandler = queueItemHandler;
        logger = loggerFactory.CreateLogger<QueueMessageHandler<TIn, TOut>>();
        this.isOutSerializable = isOutSerializable;
    }
}