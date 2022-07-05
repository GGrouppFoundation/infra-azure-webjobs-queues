using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;

namespace GGroupp.Infra;

partial class BusItemHandlerOutFunction
{
    public async Task InvokeBusItemAsync(
        [ServiceBusTrigger("%BusQueue:NameIn%")] ServiceBusReceivedMessage message,
        [ServiceBus("%BusQueue:NameOut%")] ServiceBusSender sender,
        CancellationToken cancellationToken)
    {
        _ = message ?? throw new ArgumentNullException(nameof(message));

        if (cancellationToken.IsCancellationRequested)
        {
            throw new BusItemHandlerCanceledException(messageId: message.MessageId);
        }

        try
        {
            var result = await busMessageHandler.HandleAsync(message: message, cancellationToken).ConfigureAwait(false);
            if (result.IsFailure)
            {
                return;
            }

            var outBytes = result.SuccessOrThrow();
            if (outBytes is null)
            {
                return;
            }

            var outMessage = new ServiceBusMessage(outBytes);
            await sender.SendMessageAsync(outMessage, cancellationToken).ConfigureAwait(false);
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