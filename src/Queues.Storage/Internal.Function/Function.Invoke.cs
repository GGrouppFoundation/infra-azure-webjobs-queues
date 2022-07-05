using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Context = Microsoft.Azure.WebJobs.ExecutionContext;

namespace GGroupp.Infra;

partial class QueueItemHandlerFunction
{
    public async Task InvokeQueueItemAsync([QueueTrigger("%Queue:NameIn%")] string message, string id, Context context, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new QueueItemProcessorCanceledException(messageId: id);
        }

        try
        {
            _ = await queueMessageHandler.HandleAsync(message: message, id: id, context, cancellationToken).ConfigureAwait(false);
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