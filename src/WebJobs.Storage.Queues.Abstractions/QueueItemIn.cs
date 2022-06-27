using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class QueueItemIn
{
    public QueueItemIn(string id, [AllowNull] string message)
    {
        Id = id ?? string.Empty;
        Message = message ?? string.Empty;
    }

    public string Id { get; }

    public string Message { get; }
}