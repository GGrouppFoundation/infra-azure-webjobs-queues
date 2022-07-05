namespace GGroupp.Infra;

public sealed record class QueueItemIn<T>
{
    public QueueItemIn(string messageId, T message, string correlationId, int deliveryCount, int maxDeliveryCount)
    {
        MessageId = messageId ?? string.Empty;
        Message = message;
        CorrelationId = correlationId ?? string.Empty;
        DeliveryCount = deliveryCount;
        MaxDeliveryCount = maxDeliveryCount;
    }

    public string MessageId { get; }

    public T Message { get; }

    public string CorrelationId { get; }

    public int DeliveryCount { get; }

    public int MaxDeliveryCount { get; }
}