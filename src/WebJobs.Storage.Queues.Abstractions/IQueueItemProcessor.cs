using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IQueueItemProcessor
{
    ValueTask<Result<Unit, QueueItemFailure>> ProcessAsync(QueueItemIn input, CancellationToken cancellationToken = default);
}