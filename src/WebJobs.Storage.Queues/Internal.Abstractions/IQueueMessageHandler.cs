using System;
using System.Threading;
using System.Threading.Tasks;
using Context = Microsoft.Azure.WebJobs.ExecutionContext;

namespace GGroupp.Infra;

internal interface IQueueMessageHandler
{
    ValueTask<Result<byte[]?, Unit>> HandleAsync(string message, string id, Context context, CancellationToken cancellationToken);
}