using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class QueueItemIn
{
    public QueueItemIn(string id, [AllowNull] string message, string correlationId, int deliveryCount, int maxDeliveryCount)
    {
        Id = id ?? string.Empty;
        Message = message ?? string.Empty;
        CorrelationId = correlationId ?? string.Empty;
        DeliveryCount = deliveryCount;
        MaxDeliveryCount = maxDeliveryCount;
    }

    public string Id { get; }

    public string Message { get; }

    public string CorrelationId { get; }

    public int DeliveryCount { get; }

    public int MaxDeliveryCount { get; }
}