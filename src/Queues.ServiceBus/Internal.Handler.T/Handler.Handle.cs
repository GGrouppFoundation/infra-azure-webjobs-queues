using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace GGroupp.Infra;

partial class BusMessageHandler<TIn, TOut>
{
    public async ValueTask<Result<byte[]?, Unit>> HandleAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        var result = await DeserializeMessageOrFailure(message).ForwardValueAsync(InnerHandleAsync).ConfigureAwait(false);

        return result.Forward(InnerSerializeMessageOrFailure);

        ValueTask<Result<TOut, Unit>> InnerHandleAsync(QueueItemIn<TIn> input)
            =>
            HandleAsync(input, cancellationToken);

        Result<byte[]?, Unit> InnerSerializeMessageOrFailure(TOut outMessage)
            =>
            SerializeMessageOrFailure(outMessage, message.MessageId);
    }

    private async ValueTask<Result<TOut, Unit>> HandleAsync(QueueItemIn<TIn> input, CancellationToken cancellationToken)
    {
        var result = await queueItemHandler.HandleAsync(input, cancellationToken).ConfigureAwait(false);

        return result.Map(InnerLogSuccess, InnerProcessFailure);

        TOut InnerLogSuccess(QueueItemOut<TOut> @out)
        {
            _ = logger.LogSuccess(input.MessageId);
            return @out.Message;
        }

        Unit InnerProcessFailure(QueueItemFailure failure)
            =>
            logger.ProcessFailure(failure, input.MessageId);
    }

    private Result<QueueItemIn<TIn>, Unit> DeserializeMessageOrFailure(ServiceBusReceivedMessage busMessage)
    {
        var json = busMessage.Body?.ToString();

        if (string.IsNullOrEmpty(json))
        {
            return logger.LogFailure(busMessage.MessageId, "Json message must be not empty");
        }

        try
        {
            var message = JsonSerializer.Deserialize<TIn>(json, jsonSerializerOptions);
            if (message is null)
            {
                return logger.LogFailure(busMessage.MessageId, "Message must be not null");
            }

            return new QueueItemIn<TIn>(
                messageId: busMessage.MessageId,
                message: message,
                correlationId: busMessage.CorrelationId,
                deliveryCount: busMessage.DeliveryCount,
                maxDeliveryCount: maxDeliveryCount);
        }
        catch (JsonException jsonException)
        {
            return logger.LogException(busMessage.MessageId, jsonException);
        }
        catch (NotSupportedException notSupportedException)
        {
            return logger.LogException(busMessage.MessageId, notSupportedException);
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