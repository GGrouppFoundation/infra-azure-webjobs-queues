using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Context = Microsoft.Azure.WebJobs.ExecutionContext;

namespace GGroupp.Infra;

partial class QueueItemHandlerOutFunction
{
    public async Task InvokeQueueItemQAsync(
        [QueueTrigger("%Queue:NameIn%")] string message,
        string id,
        Context context,
        [Queue("%Queue:NameOut%")] ICollector<byte[]> outputMessage,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new QueueItemProcessorCanceledException(messageId: id);
        }

        try
        {
            var result = await queueMessageHandler.HandleAsync(message: message, id: id, context, cancellationToken).ConfigureAwait(false);
            if (result.IsFailure)
            {
                return;
            }

            var outMessage = result.SuccessOrThrow();
            if (outMessage is null)
            {
                return;
            }

            outputMessage.Add(outMessage);
        }
        catch (OperationCanceledException exception)
        {
            throw new QueueItemProcessorOperationException(id, exception);
        }
        catch (Exception exception)
        {
            throw new QueueItemProcessorOperationException(queueMessageId: id, exception);
        }
    }
}