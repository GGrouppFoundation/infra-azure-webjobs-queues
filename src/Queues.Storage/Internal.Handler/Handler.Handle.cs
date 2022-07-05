using System;
using System.Threading;
using System.Threading.Tasks;
using Context = Microsoft.Azure.WebJobs.ExecutionContext;

namespace GGroupp.Infra;

partial class QueueMessageHandler
{
    public async ValueTask<Result<byte[]?, Unit>> HandleAsync(string message, string id, Context context, CancellationToken cancellationToken)
    {
        var input = new QueueItemIn(
            id: id,
            message: message,
            correlationId: context.InvocationId.ToString("D"),
            deliveryCount: context.RetryContext?.RetryCount ?? default,
            maxDeliveryCount: context.RetryContext?.MaxRetryCount ?? default);

        var result = await queueItemHandler.HandleAsync(input, cancellationToken).ConfigureAwait(false);

        return result.Map(InnerProcessSuccess, InnerProcessFailure).MapSuccess(GetBytes);

        Unit InnerProcessSuccess(Unit _)
            =>
            logger.LogSuccess(input.Id);

        Unit InnerProcessFailure(QueueItemFailure failure)
            =>
            logger.ProcessFailure(failure, input.Id);
    }

    private static byte[]? GetBytes(Unit _)
        =>
        null;
}