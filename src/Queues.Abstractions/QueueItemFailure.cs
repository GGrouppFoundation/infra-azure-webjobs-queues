using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public readonly record struct QueueItemFailure
{
    private readonly string? failureMessage;

    public QueueItemFailure([AllowNull] string failureMessage, bool returnToQueue)
    {
        this.failureMessage = string.IsNullOrEmpty(failureMessage) ? null : failureMessage;
        ReturnToQueue = returnToQueue;
    }

    public bool ReturnToQueue { get; }

    public string FailureMessage => failureMessage ?? string.Empty;
}