using System;
using Azure.Messaging.ServiceBus;

namespace GGroupp.Infra;

internal sealed record class BusQueueOptionJson
{
    public int? PrefetchCount { get; init; }

    public bool? AutoCompleteMessages { get; init; }

    public ServiceBusTransportType? TransportType { get; init; }

    public BusQueueRetryOptionJson? Retry { get; init; }

    public TimeSpan? MaxAutoLockRenewalDuration { get; init; }

    public int? MaxConcurrentCalls { get; init; }

    public int? MaxConcurrentSessions { get; init; }

    public int? MaxMessageBatchSize { get; init; }

    public TimeSpan? SessionIdleTimeout { get; init; }

    public bool? EnableCrossEntityTransactions { get; init; }
}