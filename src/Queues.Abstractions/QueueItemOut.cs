namespace GGroupp.Infra;

public sealed record class QueueItemOut<T>
{
    public QueueItemOut(T message)
        =>
        Message = message;

    public T Message { get; }
}