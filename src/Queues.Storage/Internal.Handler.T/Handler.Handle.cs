using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Context = Microsoft.Azure.WebJobs.ExecutionContext;

namespace GGroupp.Infra;

partial class QueueMessageHandler<TIn, TOut>
{
    public async ValueTask<Result<byte[]?, Unit>> HandleAsync(string message, string id, Context context, CancellationToken cancellationToken)
    {
        var result = await DeserializeMessageOrFailure(message, id, context).ForwardValueAsync(InnerHandleAsync).ConfigureAwait(false);

        return result.Forward(InnerSerializeMessageOrFailure);

        ValueTask<Result<TOut, Unit>> InnerHandleAsync(QueueItemIn<TIn> input)
            =>
            HandleAsync(input, cancellationToken);

        Result<byte[]?, Unit> InnerSerializeMessageOrFailure(TOut outMessage)
            =>
            SerializeMessageOrFailure(outMessage, id);
    }

    private async ValueTask<Result<TOut, Unit>> HandleAsync(QueueItemIn<TIn> input, CancellationToken cancellationToken)
    {
        var result = await queueItemHandler.HandleAsync(input, cancellationToken).ConfigureAwait(false);

        return result.Map(InnerLogSuccess, InnerProcessFailure);

        TOut InnerLogSuccess(QueueItemOut<TOut> @out)
            =>
            logger.LogSuccess(input.MessageId).Pipe(_ => @out.Message);

        Unit InnerProcessFailure(QueueItemFailure failure)
            =>
            logger.ProcessFailure(failure, input.MessageId);
    }

    private Result<QueueItemIn<TIn>, Unit> DeserializeMessageOrFailure(string json, string id, Context context)
    {
        if (string.IsNullOrEmpty(json))
        {
            return logger.LogFailure(id, "Json message must be not empty");
        }

        try
        {
            var message = JsonSerializer.Deserialize<TIn>(json, jsonSerializerOptions);
            if (message is null)
            {
                return logger.LogFailure(id, "Message must be not null");
            }

            return new QueueItemIn<TIn>(
                messageId: id,
                message: message,
                correlationId: context.InvocationId.ToString("D"),
                deliveryCount: context.RetryContext?.RetryCount ?? default,
                maxDeliveryCount: context.RetryContext?.MaxRetryCount ?? default);
        }
        catch (JsonException jsonException)
        {
            return logger.LogException(id, jsonException);
        }
        catch (NotSupportedException notSupportedException)
        {
            return logger.LogException(id, notSupportedException);
        }
    }

    private Result<byte[]?, Unit> SerializeMessageOrFailure(TOut message, string id)
    {
        if (isOutSerializable is false)
        {
            return null;
        }

        try
        {
            return JsonSerializer.SerializeToUtf8Bytes(message, jsonSerializerOptions);
        }
        catch (JsonException jsonException)
        {
            return logger.LogException(id, jsonException);
        }
        catch (NotSupportedException notSupportedException)
        {
            return logger.LogException(id, notSupportedException);
        }
    }
}