using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class QueueItemHandlerFunction
{
    public async Task InvokeAsync([QueueTrigger("%QueueIn:Name%")] string message, string id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new QueueItemProcessorCanceledException(messageId: id);
        }

        var input = new QueueItemIn(id: id, message: message);
        var result = await HandleAsync(input, cancellationToken).ConfigureAwait(false);

        _ = result.Fold(InnerProcessSuccess, InnerProcessFailure);

        Unit InnerProcessSuccess(Unit _)
            =>
            LogSuccessResult(id);

        Unit InnerProcessFailure(QueueItemFailure failure)
            =>
            ProcessFailure(failure, id);
    }

    private async ValueTask<Result<Unit, QueueItemFailure>> HandleAsync(QueueItemIn input, CancellationToken cancellationToken)
    {
        try
        {
            return await queueItemHandler.HandleAsync(input, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            throw new QueueItemProcessorOperationException(input.Id, exception);
        }
        catch (Exception exception)
        {
            throw new QueueItemProcessorOperationException(queueMessageId: input.Id, exception);
        }
    }

    private Unit LogSuccessResult(string messageId)
    {
        logger.LogInformation("Queue message '{id}' was proccessed successfully", messageId);
        return default;
    }

    private Unit ProcessFailure(QueueItemFailure failure, string messageId)
    {
        if (failure.ReturnToQueue)
        {
            throw new QueueItemProcessorOperationException(queueMessageId: messageId, failureMessage: failure.FailureMessage);
        }

        var id = messageId;
        var message = failure.FailureMessage;

        if (string.IsNullOrEmpty(message))
        {
            logger.LogError("Queue message '{id}' was unsuccessfully proccessed and will be deleted from the queue", id);
            return default;
        }

        logger.LogError("Queue message '{id}' was unsuccessfully proccessed and will be deleted from the queue: {message}", id, message);
        return default;
    }
}