using System;

namespace GGroupp.Infra;

public sealed record class QueueItemIn<T>
{
    public QueueItemIn(string id, T message, Guid invocationId, int retryCount, int maxRetryCount)
    {
        Id = id ?? string.Empty;
        Message = message;
        InvocationId = invocationId;
        RetryCount = retryCount;
        MaxRetryCount = maxRetryCount;
    }

    public string Id { get; }

    public T Message { get; }

    public Guid InvocationId { get; }

    public int RetryCount { get; }

    public int MaxRetryCount { get; }
}