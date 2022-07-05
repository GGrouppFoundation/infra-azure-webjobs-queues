using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IQueueItemHandler
{
    ValueTask<Result<Unit, QueueItemFailure>> HandleAsync(QueueItemIn input, CancellationToken cancellationToken = default);
}