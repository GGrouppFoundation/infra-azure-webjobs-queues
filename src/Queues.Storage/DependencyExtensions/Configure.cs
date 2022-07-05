using System;
using GGroupp.Infra;
using Microsoft.Extensions.Hosting;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class QueueItemHandlerDependencyExtensions
{
    public static IHostBuilder ConfigureQueueProcessor(this Dependency<IQueueItemHandler> dependency, IHostBuilder hostBuilder)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder.InternalConfigureQueueProcessor(dependency.Resolve);
    }
}