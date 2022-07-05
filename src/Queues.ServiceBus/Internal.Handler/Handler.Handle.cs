using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace GGroupp.Infra;

partial class BusMessageHandler
{
    public async ValueTask<Result<byte[]?, Unit>> HandleAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        var input = new QueueItemIn(
            id: message.MessageId,
            message: message.Body?.ToString() ?? string.Empty,
            correlationId: message.CorrelationId,
            deliveryCount: message.DeliveryCount,
            maxDeliveryCount: maxDeliveryCount);

        var result = await queueItemHandler.HandleAsync(input, cancellationToken).ConfigureAwait(false);

        return result.Map(InnerProcessSuccess, InnerProcessFailure).MapSuccess(NullBytes);

        Unit InnerProcessSuccess(Unit _)
            =>
            logger.LogSuccess(input.Id);

        Unit InnerProcessFailure(QueueItemFailure failure)
            =>
            logger.ProcessFailure(failure, input.Id);
    }

    private static byte[]? NullBytes(Unit _)
        =>
        null;
}