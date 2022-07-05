using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;

namespace GGroupp.Infra;

partial class BusItemHandlerFunction
{
    public async Task InvokeBusItemAsync(
        [ServiceBusTrigger("%BusQueue:NameIn%")] ServiceBusReceivedMessage message,
        CancellationToken cancellationToken)
    {
        _ = message ?? throw new ArgumentNullException(nameof(message));

        if (cancellationToken.IsCancellationRequested)
        {
            throw new BusItemHandlerCanceledException(messageId: message.MessageId);
        }

        try
        {
            _ = await busMessageHandler.HandleAsync(message: message, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            throw new BusItemHandlerOperationException(message.MessageId, exception);
        }
        catch (Exception exception)
        {
            throw new BusItemHandlerOperationException(queueMessageId: message.MessageId, exception);
        }
    }
}