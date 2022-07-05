using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace GGroupp.Infra;

internal interface IBusMessageHandler
{
    ValueTask<Result<byte[]?, Unit>> HandleAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken);
}