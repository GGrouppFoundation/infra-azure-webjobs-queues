using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IQueueItemHandler<TIn, TOut>
    where TIn : notnull
{
    ValueTask<Result<QueueItemOut<TOut>, QueueItemFailure>> HandleAsync(QueueItemIn<TIn> input, CancellationToken cancellationToken = default);
}