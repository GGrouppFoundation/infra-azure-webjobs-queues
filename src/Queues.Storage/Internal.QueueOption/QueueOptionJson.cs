using System;
using Azure.Storage.Queues;

namespace GGroupp.Infra;

internal sealed record class QueueOptionJson
{
    public int? BatchSize { get; init; }

    public int? NewBatchThreshold { get; init; }

    public TimeSpan? MaxPollingInterval { get; init; }

    public int? MaxDequeueCount { get; init; }

    public TimeSpan? VisibilityTimeout { get; init; }

    public QueueMessageEncoding? MessageEncoding { get; init; }
}