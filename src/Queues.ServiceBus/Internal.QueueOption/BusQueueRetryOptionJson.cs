using System;
using Azure.Messaging.ServiceBus;

namespace GGroupp.Infra;

internal sealed record class BusQueueRetryOptionJson
{
    public ServiceBusRetryMode? Mode { get; init; }

    public int? MaxRetries { get; init; }

    public TimeSpan? Delay { get; init; }

    public TimeSpan? MaxDelay { get; init; }

    public TimeSpan? TryTimeout { get; init; }
}